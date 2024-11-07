using System.Security.Claims;
using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.Interface;
using DivvyUp.Web.Validation;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Service
{
    public class PersonService : IPersonService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MyValidator _validator;

        public PersonService(MyDbContext dbContext, IMapper mapper, MyValidator validator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IActionResult> Add(ClaimsPrincipal claims, AddEditPersonRequest request)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsEmpty(request.Name, "Nazwa osoby jest wymagana");
                var user = await _validator.GetUser(claims);

                var newPerson = new Person()
                {
                    User = user,
                    Name = request.Name,
                    Surname = request.Surname,
                    ReceiptsCount = 0,
                    ProductsCount = 0,
                    TotalAmount = 0,
                    UnpaidAmount = 0,
                    LoanBalance = 0,
                    UserAccount = false
                };

                _dbContext.Persons.Add(newPerson);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Pomyślnie dodano osobe");
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditPersonRequest request, int personId)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsEmpty(request.Name, "Nazwa osoby jest wymagana");

                var person = await _validator.GetPerson(claims, personId);
                person.Name = request.Name;
                person.Surname = request.Surname;
                _dbContext.Persons.Update(person);

                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Pomyślnie wprowadzono zmiany");
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> Remove(ClaimsPrincipal claims, int personId)
        {
            try
            {
                _validator.IsNull(personId, "Brak identyfikatora osoby");
                var person = await _validator.GetPerson(claims, personId);

                var personProducts = await _dbContext.PersonProducts
                    .Where(pp => pp.Person.Id == personId)
                    .ToListAsync();

                if (personProducts.Any())
                {
                    return new ConflictObjectResult("Nie można usunąć osoby, która posiada przypisane produkty");
                }

                var loans = await _dbContext.Loans
                    .Where(l => l.Person.Id == personId)
                    .ToListAsync();

                if (loans.Any())
                {
                    return new ConflictObjectResult("Nie można usunąć osoby, która posiada przypisane produkty");
                }

                _dbContext.Persons.Remove(person);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Pomyślnie usunięto osobę");
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetPerson(ClaimsPrincipal claims, int personId)
        {
            try
            {
                _validator.IsNull(personId, "Brak identyfikatora osoby");

                var person = await _validator.GetPerson(claims, personId);
                var personDto = _mapper.Map<PersonDto>(person);
                return new OkObjectResult(personDto);
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetPersons(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var persons = await _dbContext.Persons
                    .Where(p => p.User == user)
                    .ToListAsync();
                var personListDto = _mapper.Map<List<PersonDto>>(persons);
                return new OkObjectResult(personListDto);
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetUserPerson(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var person = await _dbContext.Persons
                    .FirstOrDefaultAsync(p => p.User == user && p.UserAccount);
                var personDto = _mapper.Map<PersonDto>(person);
                return new OkObjectResult(personDto);
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetPersonFromReceipt(ClaimsPrincipal claims, int receiptId)
        {
            try
            {
                _validator.IsNull(receiptId, "Brak identyfikatora rachunku");

                var user = await _validator.GetUser(claims);
                await _validator.GetReceipt(claims, receiptId);

                var personProducts = await _dbContext.PersonProducts
                    .Include(pp => pp.Person)
                    .Include(pp => pp.Product)
                    .Where(pp => pp.Product.ReceiptId == receiptId && pp.Person.User == user)
                    .ToListAsync();

                var personsDto = _mapper.Map<List<PersonDto>>(personProducts.Select(pp => pp.Person).ToList());
                return new OkObjectResult(personsDto);
            }
            catch (DException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetPersonFromProduct(ClaimsPrincipal claims, int productId)
        {
            try
            {
                _validator.IsNull(productId, "Brak identyfikatora rachunku");

                var user = await _validator.GetUser(claims);
                await _validator.GetProduct(claims, productId);

                var personProducts = await _dbContext.PersonProducts
                    .Include(pp => pp.Person)
                    .Where(pp => pp.ProductId == productId && pp.Person.User == user)
                    .ToListAsync();

                var personsDto = _mapper.Map<List<PersonDto>>(personProducts.Select(pp => pp.Person).ToList());
                return new OkObjectResult(personsDto);
            }
            catch (DException ex)
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
