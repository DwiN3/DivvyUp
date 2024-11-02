using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp.Web.InterfaceWeb
{
    public interface IProductService
    {
        Task<IActionResult> Add(ClaimsPrincipal claims, AddEditProductRequest request, int receiptId);
        Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditProductRequest request, int productId);
        Task<IActionResult> Remove(ClaimsPrincipal claims, int productId);
        Task<IActionResult> SetSettled(ClaimsPrincipal claims, int productId, bool settled);
        Task<IActionResult> GetProduct(ClaimsPrincipal claims, int productId);
        Task<IActionResult> GetProducts(ClaimsPrincipal claims);
        Task<IActionResult> GetProductsFromReceipt(ClaimsPrincipal claims, int receiptId);
    }
}
