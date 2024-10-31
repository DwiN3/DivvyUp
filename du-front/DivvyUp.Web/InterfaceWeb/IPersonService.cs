using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp.Web.InterfaceWeb
{
    public interface IPersonService
    {
        Task<IActionResult> Add(AddEditPersonRequest person, ClaimsPrincipal user);
        Task<IActionResult> Edit(AddEditPersonRequest person, int personId, ClaimsPrincipal user);
        Task<IActionResult> Remove(int personId, ClaimsPrincipal user);
        Task<IActionResult> GetPerson(int personId, ClaimsPrincipal user);
        Task<IActionResult> GetPersons(ClaimsPrincipal user);
        Task<IActionResult> GetUserPerson(ClaimsPrincipal user);
        Task<IActionResult> GetPersonFromReceipt(int receiptId, ClaimsPrincipal user);
        Task<IActionResult> GetPersonFromProduct(int productId, ClaimsPrincipal user);
    }
}
