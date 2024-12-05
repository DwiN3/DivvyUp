using System.Net;
using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validation;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Services
{
    public class PersonProductService : IPersonProductService
    {
        private readonly DivvyUpDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly DValidator _validator;
        private readonly EntityUpdateService _entityUpdateService;
        private readonly UserContext _userContext;

        public PersonProductService(DivvyUpDBContext dbContext, IMapper mapper, DValidator validator, EntityUpdateService entityUpdateService, UserContext userContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
            _entityUpdateService = entityUpdateService;
            _userContext = userContext;
        }

        public async Task Add(AddEditPersonProductDto request, int productId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsNull(request.PersonId, "Brak identyfikatora osoby");
            _validator.IsNull(request.Quantity, "Ilość jest wymagana");
            _validator.IsNull(productId, "Brak identyfikatora produktu");
            var user = await _userContext.GetCurrentUser();
            var person = await _validator.GetPerson(user, request.PersonId);
            var product = await _validator.GetProduct(user, productId);

            var totalQuantity = await GetTotalQuantityByProduct(product.Id);
            if (totalQuantity + request.Quantity > product.MaxQuantity)
                throw new DException(HttpStatusCode.BadRequest, "Przekroczono maksymalną ilość produktu");

            var existingPerson = await _dbContext.PersonProducts
                .FirstOrDefaultAsync(pp => pp.ProductId == product.Id && pp.Person.Id == person.Id);
            if(existingPerson != null)
                throw new DException(HttpStatusCode.Conflict, "Osoba jest już przypisana");

            var isPersonProductCompensation = await _dbContext.PersonProducts
                .FirstOrDefaultAsync(pp => pp.ProductId == product.Id && pp.Compensation);

            var newPersonProduct = new PersonProduct()
            {
                Person = person,
                Product = product,
                Quantity = request.Quantity,
                Compensation = isPersonProductCompensation == null ? true : false,
                PartOfPrice = await _entityUpdateService.CalculatePartOfPrice(request.Quantity,product.MaxQuantity, product.Price),
                Settled = false,
            };
            _dbContext.PersonProducts.Add(newPersonProduct);

            await _dbContext.SaveChangesAsync();
            await _entityUpdateService.UpdateProductDetails(product);
            await _entityUpdateService.UpdatePerson(user, false);
        }

        public async Task Edit(AddEditPersonProductDto request, int personProductId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsNull(request.PersonId, "Brak identyfikatora osoby");
            _validator.IsNull(request.Quantity, "Ilość jest wymagana");
            _validator.IsNull(personProductId, "Brak identyfikatora powiązania produkt - osoba");
            var user = await _userContext.GetCurrentUser();
            var personProduct = await _validator.GetPersonProduct(user, personProductId);
            var person = await _validator.GetPerson(user, request.PersonId);
            var product = await _validator.GetProduct(user, personProduct.ProductId);

            personProduct.Quantity = request.Quantity;
            personProduct.Person = person;
            personProduct.PartOfPrice = await _entityUpdateService.CalculatePartOfPrice(request.Quantity, product.MaxQuantity, product.Price);
            _dbContext.PersonProducts.Update(personProduct);

            await _dbContext.SaveChangesAsync();
            await _entityUpdateService.UpdateProductDetails(product);
            await _entityUpdateService.UpdatePerson(user, false);
        }

        public async Task Remove(int personProductId)
        {
            _validator.IsNull(personProductId, "Brak identyfikatora powiązania produkt - osoba");
            var user = await _userContext.GetCurrentUser();
            var personProduct = await _validator.GetPersonProduct(user, personProductId);
            _dbContext.PersonProducts.Remove(personProduct);
            await _dbContext.SaveChangesAsync();

            if (personProduct.Compensation)
            {
                var nexProduct = await _entityUpdateService.GetPersonWithLowestCompensation(personProduct.ProductId);
                await _entityUpdateService.UpdateCompensationFlags(personProduct.ProductId, nexProduct);
            }

            await _entityUpdateService.UpdateProductDetails(personProduct.Product);
            await _entityUpdateService.UpdatePerson(user, false);
        }

        public async Task RemoveList(int productId, List<int> personProductIds)
        {
            _validator.IsNull(personProductIds, "Brak identyfikatora powiązania produkt - osoba");
            var user = await _userContext.GetCurrentUser();
            var product = await _validator.GetProduct(user, productId);

            var personProductsToRemove = new List<PersonProduct>();
            foreach (var personProductId in personProductIds)
            {
                var personProduct = await _validator.GetPersonProduct(user, personProductId);
                personProductsToRemove.Add(personProduct);
            }
            _dbContext.PersonProducts.RemoveRange(personProductsToRemove);
            await _dbContext.SaveChangesAsync();

            var productWithCompensation = await _dbContext.PersonProducts
                .FirstOrDefaultAsync(pp => pp.ProductId == product.Id && pp.Compensation);
            if (productWithCompensation == null)
            {
                var nexProduct = await _entityUpdateService.GetPersonWithLowestCompensation(productId);
                await _entityUpdateService.UpdateCompensationFlags(productId, nexProduct);
            }

            await _entityUpdateService.UpdateProductDetails(product);
            await _entityUpdateService.UpdatePerson(user, false);
        }

        public async Task SetPerson(int personProductId, int personId)
        {
            _validator.IsNull(personProductId, "Brak identyfikatora powiązania produkt - osoba");
            _validator.IsNull(personId, "Brak identyfikatora osoby");
            var user = await _userContext.GetCurrentUser();
            var personProduct = await _validator.GetPersonProduct(user, personProductId);
            var person = await _validator.GetPerson(user, personId);
            var newPerson = await _validator.GetPerson(user, personId);

            if (await IsPersonProductExists(personProduct.Product, newPerson))
            {
                throw new DException(HttpStatusCode.Conflict, "Wybrana osoba jest już przypisana do tego produktu");
            }

            personProduct.Person = person;

            _dbContext.PersonProducts.Update(personProduct);
            await _dbContext.SaveChangesAsync();
            await _entityUpdateService.UpdatePerson(user, false);
        }

        public async Task SetSettled(int personProductId, bool settled)
        {
            _validator.IsNull(personProductId, "Brak identyfikatora powiązania produkt - osoba");
            _validator.IsNull(settled, "Brak decyzji rozliczenia");
            var user = await _userContext.GetCurrentUser();

            var personProduct = await _dbContext.PersonProducts
                .Include(p => p.Product)
                .Include(p => p.Product.Receipt)
                .Include(p => p.Person)
                .FirstOrDefaultAsync(p => p.Id == personProductId);
            if (personProduct == null)
                throw new DException(HttpStatusCode.NotFound, "Przypis osoby z produktem nie znaleziony");
            if (personProduct.Person.UserId != user.Id)
                throw new DException(HttpStatusCode.Unauthorized, "Brak dostępu do przypisu produktu z osobą: " + personProductId);

            personProduct.Settled = settled;
            _dbContext.PersonProducts.Update(personProduct);

            var product = personProduct.Product;
            var allSettledPersonProduct = await _entityUpdateService.AreAllPersonProductsSettled(product);
            product.Settled = allSettledPersonProduct;
            _dbContext.Products.Update(product);

            var receipt = personProduct.Product.Receipt;
            var allSettledProduct = await _entityUpdateService.AreAllProductsSettled(receipt);
            receipt.Settled = allSettledProduct;
            _dbContext.Receipts.Update(receipt);

            await _dbContext.SaveChangesAsync();
            await _entityUpdateService.UpdatePerson(user, false);
        }
        

        public async Task SetCompensation(int personProductId)
        {
            var user = await _userContext.GetCurrentUser();
            var personProduct = await _validator.GetPersonProduct(user, personProductId);
            await _entityUpdateService.UpdateCompensationFlags(personProduct.ProductId, personProduct);
            await _entityUpdateService.UpdatePerson(user, false);
        }

        public async Task SetAutoCompensation(int productId)
        {
            var user = await _userContext.GetCurrentUser();

            var personProductWithLowestCompensation = await _entityUpdateService.GetPersonWithLowestCompensation(productId);
            if (personProductWithLowestCompensation == null)
            {
                throw new DException(HttpStatusCode.NotFound, "Brak przypisań do produktu");
            }
            await _entityUpdateService.UpdateCompensationFlags(productId, personProductWithLowestCompensation);
            await _entityUpdateService.UpdatePerson(user, false);
        }

        public async Task<PersonProductDto> GetPersonProduct(int personProductId)
        {
            _validator.IsNull(personProductId, "Brak identyfikatora powiązania produkt - osoba");
            var user = await _userContext.GetCurrentUser();
            var personProduct = await _validator.GetPersonProduct(user, personProductId);
            var personProductDto = _mapper.Map<PersonProductDto>(personProduct);
            return personProductDto;
        }

        public async Task<List<PersonProductDto>> GetPersonProductsFromPerson(int personId)
        {
            _validator.IsNull(personId, "Brak identyfikatora osoby");
            var user = await _userContext.GetCurrentUser();
            var personProducts = await _dbContext.PersonProducts
                .AsNoTracking()
                .Include(p => p.Product)
                .Include(p => p.Person)
                .Where(p => p.Person.UserId == user.Id && p.PersonId == personId)
                .ToListAsync();

            var personProductsDto = _mapper.Map<List<PersonProductDto>>(personProducts).ToList();
            return personProductsDto;
        }

        public async Task<List<PersonProductDto>> GetPersonProducts()
        {
            var user = await _userContext.GetCurrentUser();
            var personProducts = await _dbContext.PersonProducts
                .AsNoTracking()
                .Include(p => p.Product)
                .Include(p => p.Person)
                .Where(p => p.Person.UserId == user.Id)
                .ToListAsync();

            var personProductsDto = _mapper.Map<List<PersonProductDto>>(personProducts).ToList();
            return personProductsDto;
        }

        public async Task<List<PersonProductDto>> GetPersonProductsFromProduct(int productId)
        {
            _validator.IsNull(productId, "Brak identyfikatora produktu");

            var user = await _userContext.GetCurrentUser();
            var personProducts = await _dbContext.PersonProducts
                .AsNoTracking()
                .Include(p => p.Product)
                .Include(p => p.Person)
                .Where(p => p.Person.UserId == user.Id && p.ProductId == productId)
                .ToListAsync();

            var personProductsDto = _mapper.Map<List<PersonProductDto>>(personProducts).ToList();
            return personProductsDto;
        }

        private async Task<int> GetTotalQuantityByProduct(int productId)
        {
            return await _dbContext.PersonProducts
                .Where(pp => pp.ProductId == productId)
                .SumAsync(pp => pp.Quantity);
        }

        private async Task<bool> IsPersonProductExists(Product product, Person person)
        {
            return await _dbContext.PersonProducts.AnyAsync(pp => pp.ProductId == product.Id && pp.PersonId == person.Id);
        }

        private async Task<IEnumerable<PersonProduct>> GetOtherCompensations(int productId, int excludeId)
        {
            return await _dbContext.PersonProducts
                .Where(pp => pp.ProductId == productId && pp.Id != excludeId && pp.Compensation)
                .ToListAsync();
        }
    }
}
