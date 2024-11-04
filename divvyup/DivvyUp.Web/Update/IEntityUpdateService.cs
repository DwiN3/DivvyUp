using System.Security.Claims;
using DivvyUp_Shared.Model;

namespace DivvyUp.Web.Update
{
    public interface IEntityUpdateService
    {
        Task UpdatePerson(ClaimsPrincipal claims, bool updateBalance);
        Task UpdateTotalPriceReceipt(Receipt receipt);
        Task<bool> AreAllProductsSettled(Receipt receipt);
        Task UpdateCompensationPrice(Product product);
        Task<bool> AreAllPersonProductsSettled(Product product);
        Task UpdatePartPricesPersonProduct(Product product);
    }
}
