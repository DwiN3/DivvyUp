using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.Interface;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validation;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Service
{
    public class LoanService : ILoanService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MyValidator _validator;
        private readonly EntityUpdateService _entityUpdateService;

        public LoanService(MyDbContext dbContext, IMapper mapper, MyValidator validator, EntityUpdateService entityUpdateService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
            _entityUpdateService = entityUpdateService;
        }

        public async Task<IActionResult> Add(ClaimsPrincipal claims, AddEditLoanRequest request)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsNull(request.PersonId, "Osoba jest wymagana");
                _validator.IsNull(request.Amount, "Ilość jest wymagana");
                _validator.IsNull(request.Date, "Data jest wymagana");

                var person = await _validator.GetPerson(claims, request.PersonId);

                var newLoan = new Loan()
                {
                    Person = person,
                    Date = request.Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc),
                    Lent = request.Lent,
                    Amount = request.Amount,
                    Settled = false,
                };

                _dbContext.Loans.Add(newLoan);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdatePerson(claims, true);
                return new OkObjectResult("Pomyślnie dodano pożyczkę");
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

        public async Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditLoanRequest request, int loanId)
        {
            try
            {
                _validator.IsNull(request, "Nie przekazano danych");
                _validator.IsNull(request.PersonId, "Osoba jest wymagana");
                _validator.IsNull(request.Amount, "Ilość jest wymagana");
                _validator.IsNull(request.Date, "Data jest wymagana");
                _validator.IsNull(loanId, "Brak identyfikatora pożyczki");

                var loan = await _validator.GetLoan(claims, loanId);
                var person = await _validator.GetPerson(claims, request.PersonId);

                loan.Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc);
                loan.Person = person;
                loan.Amount = request.Amount;

                _dbContext.Loans.Update(loan);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdatePerson(claims, true);
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

        public async Task<IActionResult> Remove(ClaimsPrincipal claims, int loanId)
        {
            try
            {
                _validator.IsNull(loanId, "Brak identyfikatora pożyczki");

                var loan = await _validator.GetLoan(claims, loanId);

                _dbContext.Loans.Remove(loan);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdatePerson(claims, true);
                return new OkObjectResult("Pomyślnie usunięto pożyczkę");
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

        public async Task<IActionResult> SetPerson(ClaimsPrincipal claims, int loanId, int personId)
        {
            try
            {
                _validator.IsNull(loanId, "Brak identyfikatora pożyczki");
                _validator.IsNull(personId, "Brak identyfikatora osoby");

                var loan = await _validator.GetLoan(claims, loanId);
                var person = await _validator.GetPerson(claims, personId);

                loan.Person = person;

                _dbContext.Loans.Update(loan);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdatePerson(claims, true);
                return new OkObjectResult("Pomyślnie zmieniono osobe");
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

        public async Task<IActionResult> SetSettled(ClaimsPrincipal claims, int loanId, bool settled)
        {
            try
            {
                _validator.IsNull(loanId, "Brak identyfikatora pożyczki");
                _validator.IsNull(settled, "Brak decyzji rozliczenia");

                var loan = await _validator.GetLoan(claims, loanId);
                loan.Settled = settled;
                _dbContext.Loans.Update(loan);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdatePerson(claims, true);
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

        public async Task<IActionResult> SetLent(ClaimsPrincipal claims, int loanId, bool lent)
        {
            try
            {
                _validator.IsNull(loanId, "Brak identyfikatora pożyczki");
                _validator.IsNull(lent, "Brak decyzji o pożyczce");

                var loan = await _validator.GetLoan(claims, loanId);
                loan.Lent = lent;
                _dbContext.Loans.Update(loan);
                await _dbContext.SaveChangesAsync();
                await _entityUpdateService.UpdatePerson(claims, true);
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

        public async Task<IActionResult> GetLoan(ClaimsPrincipal claims, int loanId)
        {
            try
            {
                _validator.IsNull(loanId, "Brak identyfikatora pożyczki");

                var loan = await _validator.GetLoan(claims, loanId);
                var loanDto = _mapper.Map<LoanDto>(loan);
                return new OkObjectResult(loanDto);
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

        public async Task<IActionResult> GetLoans(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var loans = await _dbContext.Loans
                    .AsNoTracking()
                    .Include(p => p.Person)
                    .Include(p => p.Person.User)
                    .Where(p => p.Person.User == user)
                    .ToListAsync();

                var loansDto = _mapper.Map<List<LoanDto>>(loans).ToList();
                return new OkObjectResult(loansDto);
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

        public async Task<IActionResult> GetPersonLoans(ClaimsPrincipal claims, int personId)
        {
            try
            {
                _validator.IsNull(personId, "Brak identyfikatora osoby");

                var user = await _validator.GetUser(claims);
                var loans = await _dbContext.Loans
                    .AsNoTracking()
                    .Include(p => p.Person)
                    .Include(p => p.Person.User)
                    .Where(p => p.Person.User == user && p.Person.Id == personId)
                    .ToListAsync();

                var loansDto = _mapper.Map<List<LoanDto>>(loans).ToList();
                return new OkObjectResult(loansDto);
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

        public async Task<IActionResult> GetLoansByDataRange(ClaimsPrincipal claims, string from, string to)
        {
            try
            {
                _validator.IsEmpty(from, "Brak daty od");
                _validator.IsEmpty(to, "Brak daty do");

                if (!DateTime.TryParseExact(from, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
                    throw new ArgumentException("Nieprawidłowy format daty początkowej.");

                if (!DateTime.TryParseExact(to, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
                    throw new ArgumentException("Nieprawidłowy format daty końcowej.");

                fromDate = DateTime.SpecifyKind(fromDate, DateTimeKind.Utc);
                toDate = DateTime.SpecifyKind(toDate, DateTimeKind.Utc);

                var user = await _validator.GetUser(claims);
                var loans = await _dbContext.Loans
                    .AsNoTracking()
                    .Include(p => p.Person)
                    .Include(p => p.Person.User)
                    .Where(p => p.Person.User == user && p.Date >= fromDate && p.Date <= toDate)
                    .ToListAsync();

                var loansDto = _mapper.Map<List<LoanDto>>(loans).ToList();
                return new OkObjectResult(loansDto);
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
