using System.Net;
using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.Validation;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Service
{
    public class PersonService : IPersonService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MyValidator _validator;
        private readonly UserContext _userContext;

        public PersonService(MyDbContext dbContext, IMapper mapper, MyValidator validator, UserContext userContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
            _userContext = userContext;
        }

        public async Task Add(AddEditPersonRequest request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Name, "Nazwa osoby jest wymagana");
            var user = await _userContext.GetCurrentUser();

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
        }

        public async Task Edit(AddEditPersonRequest request, int personId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Name, "Nazwa osoby jest wymagana");
            var user = await _userContext.GetCurrentUser();
            var person = await _validator.GetPerson(user, personId);

            person.Name = request.Name;
            person.Surname = request.Surname;
            _dbContext.Persons.Update(person);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Remove(int personId)
        {
            _validator.IsNull(personId, "Brak identyfikatora osoby");
            var user = await _userContext.GetCurrentUser();
            var person = await _validator.GetPerson(user, personId);

            var personProducts = await _dbContext.PersonProducts
                .Where(pp => pp.Person.Id == personId)
                .ToListAsync();

            if (personProducts.Any())
            {
                throw new DException(HttpStatusCode.Conflict,"Nie można usunąć osoby, która posiada przypisane produkty");
            }

            var loans = await _dbContext.Loans
                .Where(l => l.Person.Id == personId)
                .ToListAsync();

            if (loans.Any())
            {
                throw new DException(HttpStatusCode.Conflict, "Nie można usunąć osoby, która posiada przypisane pożyczki");
            }

            _dbContext.Persons.Remove(person);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PersonDto> GetPerson(int personId)
        {
            _validator.IsNull(personId, "Brak identyfikatora osoby");
            var user = await _userContext.GetCurrentUser();
            var person = await _validator.GetPerson(user, personId);
            var personDto = _mapper.Map<PersonDto>(person);
            return personDto;
        }

        public async Task<List<PersonDto>> GetPersons()
        {
            var user = await _userContext.GetCurrentUser();
            var persons = await _dbContext.Persons
                .AsNoTracking()
                .Where(p => p.UserId == user.Id)
                .ToListAsync();
            var personsDto = _mapper.Map<List<PersonDto>>(persons);
            return personsDto;
        }

        public async Task<PersonDto> GetUserPerson()
        {
            var user = await _userContext.GetCurrentUser();
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.UserId == user.Id && p.UserAccount);
            var personDto = _mapper.Map<PersonDto>(person);
            return personDto;
        }

        public async Task<List<PersonDto>> GetPersonFromReceipt(int receiptId)
        {
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");

            var user = await _userContext.GetCurrentUser();
            await _validator.GetReceipt(user, receiptId);

            var personProducts = await _dbContext.PersonProducts
                .AsNoTracking()
                .Include(pp => pp.Person)
                .Include(pp => pp.Product)
                .Where(pp => pp.Product.ReceiptId == receiptId && pp.Person.UserId == user.Id)
                .ToListAsync();

            var personsDto = _mapper.Map<List<PersonDto>>(personProducts.Select(pp => pp.Person).ToList());
            return personsDto;
        }

        public async Task<List<PersonDto>> GetPersonFromProduct(int productId)
        {
            _validator.IsNull(productId, "Brak identyfikatora rachunku");

            var user = await _userContext.GetCurrentUser();
            await _validator.GetProduct(user, productId);

            var personProducts = await _dbContext.PersonProducts
                .AsNoTracking()
                .Include(pp => pp.Person)
                .Where(pp => pp.ProductId == productId && pp.Person.UserId == user.Id)
                .ToListAsync();

            var personsDto = _mapper.Map<List<PersonDto>>(personProducts.Select(pp => pp.Person).ToList());
            return personsDto;
        }
    }
}
