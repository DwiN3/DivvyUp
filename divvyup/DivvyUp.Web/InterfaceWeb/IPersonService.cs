using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp.Web.InterfaceWeb
{
    public interface IPersonService
    {
        Task<IActionResult> Add(ClaimsPrincipal claims, AddEditPersonRequest person);
        Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditPersonRequest person, int personId);
        Task<IActionResult> Remove(ClaimsPrincipal claims, int personId);
        Task<IActionResult> GetPerson(ClaimsPrincipal claims, int personId);
        Task<IActionResult> GetPersons(ClaimsPrincipal claims);
        Task<IActionResult> GetUserPerson(ClaimsPrincipal claims);
        Task<IActionResult> GetPersonFromReceipt(ClaimsPrincipal claims, int receiptId);
        Task<IActionResult> GetPersonFromProduct(ClaimsPrincipal claims, int productId);
    }
}
