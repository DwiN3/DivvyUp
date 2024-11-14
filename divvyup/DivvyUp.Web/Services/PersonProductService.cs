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
        private readonly DuDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly DuValidator _validator;
        private readonly EntityUpdateService _entityUpdateService;
        private readonly UserContext _userContext;

        public PersonProductService(DuDbContext dbContext, IMapper mapper, DuValidator validator, EntityUpdateService entityUpdateService, UserContext userContext)
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
                throw new DException(HttpStatusCode.BadRequest, "Przekroczono maksymalną ilość produktu.");

            var isFirstPersonProduct = await _dbContext.PersonProducts
                .FirstOrDefaultAsync(pp => pp.ProductId == product.Id);

            var existingPerson = await _dbContext.PersonProducts
                .FirstOrDefaultAsync(pp => pp.ProductId == product.Id && pp.Person.Id == person.Id);
            if(existingPerson != null)
                throw new DException(HttpStatusCode.Conflict, "Osoba jest już przypisana.");

            var newPersonProduct = new PersonProduct()
            {
                Person = person,
                Product = product,
                Quantity = request.Quantity,
                Compensation = isFirstPersonProduct == null ? true : false,
                PartOfPrice = CalculatePartOfPrice(product, request.Quantity),
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
            personProduct.PartOfPrice = CalculatePartOfPrice(product, request.Quantity);
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
            await _entityUpdateService.UpdateProductDetails(personProduct.Product);
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
            var otherCompensations = await GetOtherCompensations(personProduct.ProductId, personProductId);

            foreach (var compensation in otherCompensations)
            {
                compensation.Compensation = false;
                _dbContext.PersonProducts.Update(compensation);
            }

            personProduct.Compensation = true;
            _dbContext.PersonProducts.Update(personProduct);
            await _dbContext.SaveChangesAsync();
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

        private decimal CalculatePartOfPrice(Product product, int quantity)
        {
            return product.Divisible ? (product.Price / product.MaxQuantity) * quantity : product.Price;
        }

        private async Task<IEnumerable<PersonProduct>> GetOtherCompensations(int productId, int excludeId)
        {
            return await _dbContext.PersonProducts
                .Where(pp => pp.ProductId == productId && pp.Id != excludeId && pp.Compensation)
                .ToListAsync();
        }
    }
}
