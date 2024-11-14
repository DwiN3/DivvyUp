﻿using System.Net;
using AutoMapper;
using DivvyUp.Web.Data;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validation;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interface;
using DivvyUp_Shared.Model;
using DivvyUp_Shared.RequestDto;
using Microsoft.EntityFrameworkCore;


namespace DivvyUp.Web.Service
{
    public class LoanService : ILoanService
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MyValidator _validator;
        private readonly EntityUpdateService _entityUpdateService;
        private readonly UserContext _userContext;

        public LoanService(MyDbContext dbContext, IMapper mapper, MyValidator validator, EntityUpdateService entityUpdateService, UserContext userContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validator = validator;
            _entityUpdateService = entityUpdateService;
            _userContext = userContext;
        }

        public async Task Add(AddEditLoanRequest request)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsNull(request.PersonId, "Osoba jest wymagana");
            _validator.IsNull(request.Amount, "Ilość jest wymagana");
            _validator.IsNull(request.Date, "Data jest wymagana");
            var user = await _userContext.GetCurrentUser();
            var person = await _validator.GetPerson(user, request.PersonId);

            var newLoan = new Loan()
            {
                Person = person,
                Date = request.Date,
                Lent = request.Lent,
                Amount = request.Amount,
                Settled = false,
            };

            _dbContext.Loans.Add(newLoan);
            await _dbContext.SaveChangesAsync();
            await _entityUpdateService.UpdatePerson(user, true);
        }

        public async Task Edit(AddEditLoanRequest request, int loanId)
        {
            _validator.IsNull(request, "Nie przekazano danych");
            _validator.IsNull(request.PersonId, "Osoba jest wymagana");
            _validator.IsNull(request.Amount, "Ilość jest wymagana");
            _validator.IsNull(request.Date, "Data jest wymagana");
            _validator.IsNull(loanId, "Brak identyfikatora pożyczki");
            var user = await _userContext.GetCurrentUser();
            var loan = await _validator.GetLoan(user, loanId);
            var person = await _validator.GetPerson(user, request.PersonId);

            loan.Date = request.Date;
            loan.Person = person;
            loan.Amount = request.Amount;

            _dbContext.Loans.Update(loan);
            await _dbContext.SaveChangesAsync();
            await _entityUpdateService.UpdatePerson(user, true);
        }

        public async Task Remove(int loanId)
        {
            _validator.IsNull(loanId, "Brak identyfikatora pożyczki");
            var user = await _userContext.GetCurrentUser();
            var loan = await _validator.GetLoan(user, loanId);

            _dbContext.Loans.Remove(loan);
            await _dbContext.SaveChangesAsync();
            await _entityUpdateService.UpdatePerson(user, true);
        }

        public async Task SetPerson(int loanId, int personId)
        {
            _validator.IsNull(loanId, "Brak identyfikatora pożyczki");
            _validator.IsNull(personId, "Brak identyfikatora osoby");
            var user = await _userContext.GetCurrentUser();
            var loan = await _validator.GetLoan(user, loanId);
            var person = await _validator.GetPerson(user, personId);

            loan.Person = person;

            _dbContext.Loans.Update(loan);
            await _dbContext.SaveChangesAsync();
            await _entityUpdateService.UpdatePerson(user, true);
        }

        public async Task SetSettled(int loanId, bool settled)
        {
            _validator.IsNull(loanId, "Brak identyfikatora pożyczki");
            _validator.IsNull(settled, "Brak decyzji rozliczenia");
            var user = await _userContext.GetCurrentUser();
            var loan = await _validator.GetLoan(user, loanId);

            loan.Settled = settled;

            _dbContext.Loans.Update(loan);
            await _dbContext.SaveChangesAsync();
            await _entityUpdateService.UpdatePerson(user, true);
        }

        public async Task SetLent(int loanId, bool lent)
        {
            _validator.IsNull(loanId, "Brak identyfikatora pożyczki");
            _validator.IsNull(lent, "Brak decyzji o pożyczce");
            var user = await _userContext.GetCurrentUser();
            var loan = await _validator.GetLoan(user, loanId);

            loan.Lent = lent;
            _dbContext.Loans.Update(loan);
            await _dbContext.SaveChangesAsync();
            await _entityUpdateService.UpdatePerson(user, true);
        }

        public async Task<LoanDto> GetLoan(int loanId)
        {
            _validator.IsNull(loanId, "Brak identyfikatora pożyczki");
            var user = await _userContext.GetCurrentUser();

            var loan = await _validator.GetLoan(user, loanId);
            var loanDto = _mapper.Map<LoanDto>(loan);
            return loanDto;
        }

        public async Task<List<LoanDto>> GetLoans()
        {
            var user = await _userContext.GetCurrentUser();

            var loans = await _dbContext.Loans
                .AsNoTracking()
                .Include(p => p.Person)
                .Where(p => p.Person.UserId == user.Id)
                .ToListAsync();

            var loansDto = _mapper.Map<List<LoanDto>>(loans).ToList();
            return loansDto;
        }

        public async Task<List<LoanDto>> GetPersonLoans(int personId)
        {
            _validator.IsNull(personId, "Brak identyfikatora osoby");

            var user = await _userContext.GetCurrentUser();
            var loans = await _dbContext.Loans
                    .AsNoTracking()
                    .Include(p => p.Person)
                    .Where(p => p.Person.UserId == user.Id && p.Person.Id == personId)
                    .ToListAsync();

            var loansDto = _mapper.Map<List<LoanDto>>(loans).ToList();
            return loansDto;
        }

        public async Task<List<LoanDto>> GetLoansByDataRange(DateOnly from, DateOnly to)
        {
            _validator.IsNull(from, "Brak daty od");
            _validator.IsNull(to, "Brak daty do");
            var user = await _userContext.GetCurrentUser();

            if (from > to)
            {
                throw new DException(HttpStatusCode.BadRequest,"Data od nie może być późniejsza niż data do");
            }

            var loans = await _dbContext.Loans
                .AsNoTracking()
                .Include(p => p.Person)
                .Where(p => p.Person.UserId == user.Id && p.Date >= from && p.Date <= to)
                .ToListAsync();

            var loansDto = _mapper.Map<List<LoanDto>>(loans).ToList();
            return loansDto;
        }
    }
}
