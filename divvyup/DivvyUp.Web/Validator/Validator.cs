using System.Net;
using DivvyUp_Shared.Model;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DivvyUp.Web.Validator
{
    public class Validator : IValidator
    {
        private readonly MyDbContext _dbContext;

        public Validator(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUser(ClaimsPrincipal claims)
        {
            var userIdClaim = claims.FindFirst("Id")?.Value;
            if (userIdClaim == null)
                throw new ValidException(HttpStatusCode.Unauthorized,"Błędny token");

            var userId = int.Parse(userIdClaim);
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                throw new ValidException(HttpStatusCode.NotFound,"Nie znaleziono osoby");

            return user;
        }

        public async Task<Person> GetPerson(ClaimsPrincipal claims, int personId)
        {
            var user = await GetUser(claims);

            var person = await _dbContext.Persons
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
                throw new ValidException(HttpStatusCode.NotFound,"Osoba nie znaleziona");
            if (person.User.Id != user.Id)
                throw new ValidException(HttpStatusCode.Unauthorized,"Brak dostępu do osoby: "+person.Id);

            return person;
        }

        public async Task<Receipt> GetReceipt(ClaimsPrincipal claims, int receiptId)
        {
            var user = await GetUser(claims);

            var receipt = await _dbContext.Receipts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == receiptId);
            if (receipt == null)
                throw new ValidException(HttpStatusCode.NotFound, "Rachunek nie znaleziony");
            if (receipt.User.Id != user.Id)
                throw new ValidException(HttpStatusCode.Unauthorized, "Brak dostępu do osoby: " + receipt.Id);

            return receipt;
        }

        public async Task<Loan> GetLoan(ClaimsPrincipal claims, int loanId)
        {
            var user = await GetUser(claims);

            var loan = await _dbContext.Loans
                .Include(p => p.Person)
                .Include(p => p.Person.User)
                .FirstOrDefaultAsync(p => p.Id == loanId);
            if (loan == null)
                throw new ValidException(HttpStatusCode.NotFound, "Rachunek nie znaleziony");
            if (loan.Person.User.Id != user.Id)
                throw new ValidException(HttpStatusCode.Unauthorized, "Brak dostępu do osoby: " + loan.Id);

            return loan;
        }
    }
}
