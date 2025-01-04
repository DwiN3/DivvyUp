using System.Net;
using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.EntityManager;
using DivvyUp.Web.Validation;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DivvyUp.Web.Services
{
    public class PersonService : IPersonService
    {
        private readonly DivvyUpDBContext _dbContext;
        private readonly EntityManagementService _managementService;
        private readonly DValidator _validator;
        private readonly IMapper _mapper;

        public PersonService(DivvyUpDBContext dbContext, EntityManagementService managementService, DValidator validator, IMapper mapper)
        {
            _dbContext = dbContext;
            _managementService = managementService;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task Add(AddEditPersonDto request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Name, "Nazwa osoby jest wymagana");
            var user = await _managementService.GetUser();

            var exitingPerson = await _dbContext.Persons
                .Where(pp => pp.Name.Equals(request.Name) && pp.Surname.Equals(request.Surname) && pp.UserId == user.Id)
                .ToListAsync();
            if (exitingPerson.Any())
            {
                throw new DException(HttpStatusCode.Conflict, "Taka osoba jest już dodana do konta");
            }

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

        public async Task Edit(AddEditPersonDto request, int personId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsEmpty(request.Name, "Nazwa osoby jest wymagana");
            var user = await _managementService.GetUser();
            var person = await _managementService.GetPerson(user, personId);

            person.Name = request.Name;
            person.Surname = request.Surname;
            _dbContext.Persons.Update(person);

            if (person.UserAccount)
            {
                user.Name = request.Name;
                user.Surname = request.Surname;
                _dbContext.Users.Update(user);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task Remove(int personId)
        {
            _validator.IsNull(personId, "Brak identyfikatora osoby");
            var user = await _managementService.GetUser();
            var person = await _managementService.GetPerson(user, personId);

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
            var user = await _managementService.GetUser();
            var person = await _managementService.GetPerson(user, personId);
            var personDto = _mapper.Map<PersonDto>(person);
            return personDto;
        }

        public async Task<List<PersonDto>> GetPersons()
        {
            var user = await _managementService.GetUser();
            var persons = await _dbContext.Persons
                .AsNoTracking()
                .Where(p => p.UserId == user.Id)
                .ToListAsync();
            var personsDto = _mapper.Map<List<PersonDto>>(persons);
            return personsDto;
        }

        public async Task<PersonDto> GetUserPerson()
        {
            var user = await _managementService.GetUser();
            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.UserId == user.Id && p.UserAccount);
            var personDto = _mapper.Map<PersonDto>(person);
            return personDto;
        }

        public async Task<List<PersonDto>> GetPersonFromReceipt(int receiptId)
        {
            _validator.IsNull(receiptId, "Brak identyfikatora rachunku");

            var user = await _managementService.GetUser();
            await _managementService.GetReceipt(user, receiptId);

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

            var user = await _managementService.GetUser();
            await _managementService.GetProduct(user, productId);

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
