using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp.Web.InterfaceWeb
{
    public interface IReceiptService
    {
        Task<IActionResult> Add(ClaimsPrincipal claims, AddEditReceiptRequest request);
        Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditReceiptRequest request, int receiptId);
        Task<IActionResult> Remove(ClaimsPrincipal claims, int receiptId);
        Task<IActionResult> SetSettled(ClaimsPrincipal claims, int receiptId, bool settled);
        Task<IActionResult> GetReceipt(ClaimsPrincipal claims, int receiptId);
        Task<IActionResult> GetReceipts(ClaimsPrincipal claims);
        Task<IActionResult> GetReceiptsByDataRange(ClaimsPrincipal claims, string from, string to);
    }
}
