using System.Security.Claims;
using AutoMapper;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validator;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Service
{
    public class PersonProductService : IPersonProductService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator _validator;
        private readonly IEntityUpdateService _entityUpdateService;

        public PersonProductService(MyDbContext dbContext, IMapper mapper, IValidator validator, IEntityUpdateService entityUpdateService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
            _entityUpdateService = entityUpdateService;
        }

        public async Task<IActionResult> Add(ClaimsPrincipal claims, AddEditPersonProductRequest request, int productId)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsNull(request.PersonId, "Brak identyfikatora osoby");
                _validator.IsNull(request.Quantity, "Ilość jest wymagana");
                _validator.IsNull(productId, "Brak identyfikatora produktu");

                var person = await _validator.GetPerson(claims, request.PersonId);
                var product = await _validator.GetProduct(claims, productId);

                var totalQuantity = await GetTotalQuantityByProduct(product.Id);
                if (totalQuantity + request.Quantity > product.MaxQuantity)
                    return new BadRequestObjectResult("Przekroczono maksymalną ilość produktu.");

                var existingPersonProduct = await _dbContext.PersonProducts
                    .FirstOrDefaultAsync(pp => pp.ProductId == product.Id && pp.PersonId == person.Id);

                var newPersonProduct = new PersonProduct()
                {
                    Person = person,
                    PersonId = person.Id,
                    Product = product,
                    ProductId = product.Id,
                    Quantity = request.Quantity,
                    Compensation = existingPersonProduct == null ? true : false,
                    PartOfPrice = CalculatePartOfPrice(product, request.Quantity),
                    Settled = false,
                };

                _dbContext.PersonProducts.Add(newPersonProduct);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdateCompensationPrice(product);
                await _entityUpdateService.UpdatePerson(claims, false);
                return new OkObjectResult("Pomyślnie dodano przypis produktu do osoby");
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditPersonProductRequest request, int personProductId)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsNull(request.PersonId, "Brak identyfikatora osoby");
                _validator.IsNull(request.Quantity, "Ilość jest wymagana");
                _validator.IsNull(personProductId, "Brak identyfikatora powiązania produkt - osoba");

                var personProduct = await _validator.GetPersonProduct(claims, personProductId);
                var person = await _validator.GetPerson(claims, request.PersonId);
                var product = await _validator.GetProduct(claims, personProduct.ProductId);

                personProduct.Quantity = request.Quantity;
                personProduct.Person = person;
                personProduct.PersonId = request.PersonId;
                personProduct.PartOfPrice = CalculatePartOfPrice(product, request.Quantity);

                _dbContext.PersonProducts.Update(personProduct);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdateCompensationPrice(product);
                await _entityUpdateService.UpdatePerson(claims, false);
                return new OkObjectResult("Pomyślnie wprowadzono zmiany");
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> Remove(ClaimsPrincipal claims, int personProductId)
        {
            try
            {
                _validator.IsNull(personProductId, "Brak identyfikatora powiązania produkt - osoba");

                var personProduct = await _validator.GetPersonProduct(claims, personProductId);

                _dbContext.PersonProducts.Remove(personProduct);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdateCompensationPrice(personProduct.Product);
                await _entityUpdateService.UpdatePerson(claims, false);
                return new OkObjectResult("Pomyślnie usunięto przypis produktu do osoby");
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> SetPerson(ClaimsPrincipal claims, int personProductId, int personId)
        {
            try
            {
                _validator.IsNull(personProductId, "Brak identyfikatora powiązania produkt - osoba");
                _validator.IsNull(personId, "Brak identyfikatora osoby");

                var personProduct = await _validator.GetPersonProduct(claims, personProductId);
                var person = await _validator.GetPerson(claims, personId);
                var newPerson = await _validator.GetPerson(claims, personId);

                if (await IsPersonProductExists(personProduct.Product, newPerson))
                {
                    return new ConflictObjectResult("Wybrana osoba jest już przypisana do tego produktu");
                }

                personProduct.Person = person;
                personProduct.PersonId = personId;

                _dbContext.PersonProducts.Update(personProduct);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdatePerson(claims, false);
                return new OkObjectResult("Pomyślnie wprowadzono zmiany");
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> SetSettled(ClaimsPrincipal claims, int personProductId, bool settled)
        {
            try
            {
                _validator.IsNull(personProductId, "Brak identyfikatora powiązania produkt - osoba");
                _validator.IsNull(settled, "Brak decyzji rozliczenia");

                var personProduct = await _validator.GetPersonProduct(claims, personProductId);
                personProduct.Settled = settled;

                _dbContext.PersonProducts.Update(personProduct);
                await _dbContext.SaveChangesAsync();

                var allSettled = await AreAllPersonProductsSettled(personProduct.ProductId);
                //await _entityUpdateService.UpdateProductSettledStatus(personProduct.ProductId, allSettled);
                return new OkObjectResult("Pomyślnie wprowadzono zmiany");
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> SetCompensation(ClaimsPrincipal claims, int personProductId)
        {
            try
            {
                var personProduct = await _validator.GetPersonProduct(claims, personProductId);
                var otherCompensations = await GetOtherCompensations(personProduct.ProductId, personProductId);

                foreach (var compensation in otherCompensations)
                {
                    compensation.Compensation = false;
                    _dbContext.PersonProducts.Update(compensation);
                }

                personProduct.Compensation = true;
                _dbContext.PersonProducts.Update(personProduct);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdatePerson(claims, false);

                return new OkObjectResult("Pomyślnie zaktualizowano stan kompensacji.");
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetPersonProduct(ClaimsPrincipal claims, int personProductId)
        {
            try
            {
                _validator.IsNull(personProductId, "Brak identyfikatora powiązania produkt - osoba");
            
                var personProduct = await _validator.GetPersonProduct(claims, personProductId);
                var personProductDto = _mapper.Map<PersonProductDto>(personProduct);
                return new OkObjectResult(personProductDto);
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetPersonProducts(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var personProducts = await _dbContext.PersonProducts
                    .Include(p => p.Product)
                    .Include(p => p.Person)
                    .Include(p => p.Person.User)
                    .Where(p => p.Person.User == user)
                    .ToListAsync();

                var personProductsDto = _mapper.Map<List<PersonProductDto>>(personProducts).ToList();
                return new OkObjectResult(personProductsDto);
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetPersonProductsFromProduct(ClaimsPrincipal claims, int productId)
        {
            try
            {
                _validator.IsNull(productId, "Brak identyfikatora produktu");

                var user = await _validator.GetUser(claims);
                var personProducts = await _dbContext.PersonProducts
                    .Include(p => p.Product)
                    .Include(p => p.Person)
                    .Include(p => p.Person.User)
                    .Where(p => p.Person.User == user && p.ProductId == productId)
                    .ToListAsync();

                var personProductsDto = _mapper.Map<List<PersonProductDto>>(personProducts).ToList();
                return new OkObjectResult(personProductsDto);
            }
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
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

        private async Task<bool> AreAllPersonProductsSettled(int productId)
        {
            return await _dbContext.PersonProducts
                .Where(pp => pp.ProductId == productId)
                .AllAsync(pp => pp.Settled);
        }
    }
}
