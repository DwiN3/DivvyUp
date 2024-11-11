using Microsoft.AspNetCore.Mvc;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp.Web.Interface
{
    public interface IReceiptService
    {
        Task<IActionResult> Add(AddEditReceiptRequest request);
        Task<IActionResult> Edit(AddEditReceiptRequest request, int receiptId);
        Task<IActionResult> Remove(int receiptId);
        Task<IActionResult> SetSettled(int receiptId, bool settled);
        Task<ActionResult<ReceiptDto>> GetReceipt(int receiptId);
        Task<ActionResult<List<ReceiptDto>>> GetReceipts();
        Task<ActionResult<List<ReceiptDto>>> GetReceiptsByDataRange(string from, string to);
    }
}
