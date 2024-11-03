using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AutoMapper;
using Azure.Core;
using DivvyUp.Web.InterfaceWeb;
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

        public PersonProductService(MyDbContext dbContext, IMapper mapper, IValidator validator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
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

                var newPersonProduct = new PersonProduct()
                {
                    Person = person,
                    PersonId = person.Id,
                    Product = product,
                    ProductId = product.Id,
                    Quantity = request.Quantity,
                    Compensation = false,
                    PartOfPrice = 0,
                    Settled = false,
                };

                _dbContext.PersonProducts.Add(newPersonProduct);
                await _dbContext.SaveChangesAsync();
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

                personProduct.Quantity = request.Quantity;
                personProduct.Person = person;
                personProduct.PersonId = request.PersonId;

                _dbContext.PersonProducts.Update(personProduct);
                await _dbContext.SaveChangesAsync();
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

                personProduct.Person = person;
                personProduct.PersonId = personId;

                _dbContext.PersonProducts.Update(personProduct);
                await _dbContext.SaveChangesAsync();
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
                _validator.IsNull(personProductId, "Brak identyfikatora powiązania produkt - osoba");

                var personProduct = await _validator.GetPersonProduct(claims, personProductId);
                personProduct.Compensation = true;

                _dbContext.PersonProducts.Update(personProduct);
                await _dbContext.SaveChangesAsync();
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
    }
}
