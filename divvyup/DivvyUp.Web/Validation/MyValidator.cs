using System.Net;
using DivvyUp_Shared.Model;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DivvyUp.Web.Data;
using DivvyUp_Impl_Maui.Api.Exceptions;

namespace DivvyUp.Web.Validation
{
    public class MyValidator
    {
        private readonly MyDbContext _dbContext;

        public MyValidator(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void IsNull(object obj, string message)
        {
            if (obj == null)
            {
                throw new DException(HttpStatusCode.BadRequest, message);
            }
        }

        public void IsEmpty(string str, string message)
        {
            if (str.IsNullOrEmpty())
            {
                throw new DException(HttpStatusCode.BadRequest, message);
            }
        }

        public async Task<User> GetUser(ClaimsPrincipal claims)
        {
            var userIdClaim = claims.FindFirst("Id")?.Value;
            if (userIdClaim == null)
                throw new DException(HttpStatusCode.Unauthorized,"Błędny token");

            var userId = int.Parse(userIdClaim);
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                throw new DException(HttpStatusCode.NotFound,"Nie znaleziono osoby");

            return user;
        }

        public async Task<Person> GetPerson(ClaimsPrincipal claims, int personId)
        {
            var user = await GetUser(claims);

            var person = await _dbContext.Persons.FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
                throw new DException(HttpStatusCode.NotFound,"Osoba nie znaleziona");
            if (person.UserId != user.Id)
                throw new DException(HttpStatusCode.Unauthorized,"Brak dostępu do osoby: "+person.Id);

            return person;
        }

        public async Task<Loan> GetLoan(ClaimsPrincipal claims, int loanId)
        {
            var user = await GetUser(claims);

            var loan = await _dbContext.Loans
                .Include(p => p.Person)
                .FirstOrDefaultAsync(p => p.Id == loanId);
            if (loan == null)
                throw new DException(HttpStatusCode.NotFound, "Pożyczka nie znaleziona");
            if (loan.Person.UserId != user.Id)
                throw new DException(HttpStatusCode.Unauthorized, "Brak dostępu do pożyczki: " + loan.Id);

            return loan;
        }

        public async Task<Receipt> GetReceipt(ClaimsPrincipal claims, int receiptId)
        {
            var user = await GetUser(claims);

            var receipt = await _dbContext.Receipts.FirstOrDefaultAsync(p => p.Id == receiptId);
            if (receipt == null)
                throw new DException(HttpStatusCode.NotFound, "Rachunek nie znaleziony");
            if (receipt.UserId != user.Id)
                throw new DException(HttpStatusCode.Unauthorized, "Brak dostępu do rachunku: " + receipt.Id);

            return receipt;
        }

        public async Task<Product> GetProduct(ClaimsPrincipal claims, int productId)
        {
            var user = await GetUser(claims);

            var product = await _dbContext.Products
                .Include(p => p.Receipt)
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                throw new DException(HttpStatusCode.NotFound, "Produkt nie znaleziony");
            if (product.Receipt.UserId != user.Id)
                throw new DException(HttpStatusCode.Unauthorized, "Brak dostępu do produktu: " + product.Id);

            return product;
        }

        public async Task<PersonProduct> GetPersonProduct(ClaimsPrincipal claims, int personProductId)
        {
            var user = await GetUser(claims);

            var personProduct = await _dbContext.PersonProducts
                .Include(p => p.Product)
                .Include(p => p.Person)
                .FirstOrDefaultAsync(p => p.Id == personProductId);
            if (personProduct == null)
                throw new DException(HttpStatusCode.NotFound, "Przypis osoby z produktem nie znaleziony");
            if (personProduct.Person.UserId != user.Id)
                throw new DException(HttpStatusCode.Unauthorized, "Brak dostępu do przypisu produktu z osobą: " + personProductId);

            return personProduct;
        }
    }
}
