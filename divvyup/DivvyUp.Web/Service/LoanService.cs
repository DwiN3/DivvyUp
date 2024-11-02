﻿using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp.Web.Validator;
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
        private readonly IValidator _validator;

        public LoanService(MyDbContext dbContext, IMapper mapper, IValidator validator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IActionResult> Add(ClaimsPrincipal claims, AddEditLoanRequest request)
        {
            try
            {
                var person = await _validator.GetPerson(claims, request.PersonId);

                var newLoan = new Loan()
                {
                    Person = person,
                    PersonId = person.Id,
                    Date = request.Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc),
                    Lent = request.Lent,
                    Amount = request.Amount,
                    Settled = false,
                };

                _dbContext.Loans.Add(newLoan);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Pomyślnie dodano pożyczkę");
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

        public async Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditLoanRequest request, int loanId)
        {
            try
            {
                var loan = await _validator.GetLoan(claims, loanId);
                var person = await _validator.GetPerson(claims, request.PersonId);

                loan.Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc);
                loan.Person = person;
                loan.PersonId = request.PersonId;
                loan.Amount = request.Amount;

                _dbContext.Loans.Update(loan);
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

        public async Task<IActionResult> Remove(ClaimsPrincipal claims, int loanId)
        {
            try
            {
                var loan = await _validator.GetLoan(claims, loanId);

                _dbContext.Loans.Remove(loan);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Pomyślnie usunięto pożyczkę");
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

        public async Task<IActionResult> SetPerson(ClaimsPrincipal claims, int loanId, int personId)
        {
            try
            {
                var loan = await _validator.GetLoan(claims, loanId);
                var person = await _validator.GetPerson(claims, personId);

                loan.Person = person;
                loan.PersonId = personId;

                _dbContext.Loans.Update(loan);
                await _dbContext.SaveChangesAsync();
                return new OkObjectResult("Pomyślnie zmieniono osobe");
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

        public async Task<IActionResult> SetSettled(ClaimsPrincipal claims, int loanId, bool settled)
        {
            try
            {
                var loan = await _validator.GetLoan(claims, loanId);
                loan.Settled = settled;
                _dbContext.Loans.Update(loan);
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

        public async Task<IActionResult> SetLent(ClaimsPrincipal claims, int loanId, bool lent)
        {
            try
            {
                var loan = await _validator.GetLoan(claims, loanId);
                loan.Lent = lent;
                _dbContext.Loans.Update(loan);
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

        public async Task<IActionResult> GetLoan(ClaimsPrincipal claims, int personId)
        {
            try
            {
                var loan = await _validator.GetLoan(claims, personId);
                var loanDto = _mapper.Map<LoanDto>(loan);
                return new OkObjectResult(loanDto);
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

        public async Task<IActionResult> GetLoans(ClaimsPrincipal claims)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var loans = await _dbContext.Loans
                    .Include(p => p.Person)
                    .Include(p => p.Person.User)
                    .Where(p => p.Person.User == user)
                    .ToListAsync();

                var loansDto = _mapper.Map<List<LoanDto>>(loans).ToList();
                return new OkObjectResult(loansDto);
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

        public async Task<IActionResult> GetPersonLoans(ClaimsPrincipal claims, int personId)
        {
            try
            {
                var user = await _validator.GetUser(claims);
                var loans = await _dbContext.Loans
                    .Include(p => p.Person)
                    .Include(p => p.Person.User)
                    .Where(p => p.Person.User == user && p.Person.Id == personId)
                    .ToListAsync();

                var loansDto = _mapper.Map<List<LoanDto>>(loans).ToList();
                return new OkObjectResult(loansDto);
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

        public async Task<IActionResult> GetLoansByDataRange(ClaimsPrincipal claims, string from, string to)
        {
            try
            {
                if (!DateTime.TryParseExact(from, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
                    throw new ArgumentException("Nieprawidłowy format daty początkowej.");

                if (!DateTime.TryParseExact(to, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
                    throw new ArgumentException("Nieprawidłowy format daty końcowej.");

                fromDate = DateTime.SpecifyKind(fromDate, DateTimeKind.Utc);
                toDate = DateTime.SpecifyKind(toDate, DateTimeKind.Utc);

                var user = await _validator.GetUser(claims);
                var loans = await _dbContext.Loans
                    .Include(p => p.Person)
                    .Include(p => p.Person.User)
                    .Where(p => p.Person.User == user && p.Date >= fromDate && p.Date <= toDate)
                    .ToListAsync();

                var loansDto = _mapper.Map<List<LoanDto>>(loans).ToList();
                return new OkObjectResult(loansDto);
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
