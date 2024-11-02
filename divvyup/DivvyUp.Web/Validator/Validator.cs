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

        public async Task<Loan> GetLoan(ClaimsPrincipal claims, int loanId)
        {
            var user = await GetUser(claims);

            var loan = await _dbContext.Loans
                .Include(p => p.Person)
                .Include(p => p.Person.User)
                .FirstOrDefaultAsync(p => p.Id == loanId);
            if (loan == null)
                throw new ValidException(HttpStatusCode.NotFound, "Pożyczka nie znaleziona");
            if (loan.Person.User.Id != user.Id)
                throw new ValidException(HttpStatusCode.Unauthorized, "Brak dostępu do pożyczki: " + loan.Id);

            return loan;
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
                throw new ValidException(HttpStatusCode.Unauthorized, "Brak dostępu do rachunku: " + receipt.Id);

            return receipt;
        }

        public async Task<Product> GetProduct(ClaimsPrincipal claims, int productId)
        {
            var user = await GetUser(claims);

            var product = await _dbContext.Products
                .Include(p => p.Receipt)
                .Include(p => p.Receipt.User)
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                throw new ValidException(HttpStatusCode.NotFound, "Produkt nie znaleziony");
            if (product.Receipt.User.Id != user.Id)
                throw new ValidException(HttpStatusCode.Unauthorized, "Brak dostępu do produktu: " + product.Id);

            return product;
        }

        public async Task<PersonProduct> GetPersonProduct(ClaimsPrincipal claims, int personProductId)
        {
            var user = await GetUser(claims);

            var personProduct = await _dbContext.PersonProducts
                .Include(p => p.Product)
                .Include(p => p.Person)
                .Include(p => p.Person.User)
                .FirstOrDefaultAsync(p => p.Id == personProductId);
            if (personProduct == null)
                throw new ValidException(HttpStatusCode.NotFound, "Przypis osoby z produktem nie znaleziony");
            if (personProduct.Person.User.Id != user.Id)
                throw new ValidException(HttpStatusCode.Unauthorized, "Brak dostępu do przypisu produktu z osobą: " + personProductId);

            return personProduct;
        }
    }
}
