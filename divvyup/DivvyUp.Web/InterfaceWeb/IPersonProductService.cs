using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp.Web.InterfaceWeb
{
    public interface IPersonProductService
    {
        Task<IActionResult> Add(ClaimsPrincipal claims, AddEditPersonProductRequest request, int productId);
        Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditPersonProductRequest request, int personProductId);
        Task<IActionResult> Remove(ClaimsPrincipal claims, int personProductId);
        Task<IActionResult> SetPerson(ClaimsPrincipal claims, int personProductId, int personId);
        Task<IActionResult> SetSettled(ClaimsPrincipal claims, int personProductId, bool settled);
        Task<IActionResult> SetCompensation(ClaimsPrincipal claims, int personProductId);
        Task<IActionResult> GetPersonProduct(ClaimsPrincipal claims, int personProductId);
        Task<IActionResult> GetPersonProducts(ClaimsPrincipal claims);
        Task<IActionResult> GetPersonProductsFromProduct(ClaimsPrincipal claims, int productId);
    }
}
