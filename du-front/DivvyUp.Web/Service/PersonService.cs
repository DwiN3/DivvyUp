using System.Security.Claims;
using AutoMapper;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp.Web.Validator;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Person = DivvyUp_Shared.Model.Person;


namespace DivvyUp.Web.Service
{
    public class PersonService : IPersonService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator _validator;

        public PersonService(MyDbContext dbContext, IMapper mapper, IValidator validator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IActionResult> Add(AddEditPersonRequest person, ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);

                var newPerson = new Person()
                {
                    User = user,
                    Name = person.Name,
                    Surname = person.Surname,
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
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> Edit(AddEditPersonRequest request, int personId, ClaimsPrincipal claims)
        {
            try
            {
                var person = await _validator.GetPerson(claims, personId);
                person.Name = request.Name;
                person.Surname = request.Surname;
                _dbContext.Persons.Update(person);
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

        public async Task<IActionResult> Remove(int personId, ClaimsPrincipal claims)
        {
            try
            {
                var person = await _validator.GetPerson(claims, personId);

                _dbContext.Persons.Remove(person);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Usunięto osobe");
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

        public async Task<IActionResult> GetPerson(int personId, ClaimsPrincipal claims)
        {
            try
            {
                var person = await _validator.GetPerson(claims, personId);
                var personDto = _mapper.Map<PersonDto>(person);
                return new OkObjectResult(personDto);
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
            catch (ValidException ex)
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
            catch (ValidException ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)ex.Status };
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }


        public Task<IActionResult> GetPersonFromReceipt(int receiptId, ClaimsPrincipal claims)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetPersonFromProduct(int productId, ClaimsPrincipal claims)
        {
            throw new NotImplementedException();
        }
    }
}
