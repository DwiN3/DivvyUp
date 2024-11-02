using System.Security.Claims;
using DivvyUp_Shared.Model;

namespace DivvyUp.Web.Validator
{
    public interface IValidator
    {
        Task<User> GetUser(ClaimsPrincipal claims);
        Task<Person> GetPerson(ClaimsPrincipal claims, int personId);
        Task<Loan> GetLoan(ClaimsPrincipal claims, int loanId);
        Task<Receipt> GetReceipt(ClaimsPrincipal claims, int receiptId);
        Task<Product> GetProduct(ClaimsPrincipal claims, int productId);
        Task<PersonProduct> GetPersonProduct(ClaimsPrincipal claims, int personProductId);
    }
}
