using System.Runtime.InteropServices.JavaScript;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route(ApiRoute.RECEIPT_ROUTES.RECEIPT_ROUTE)]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [Authorize]
        [HttpPost(ApiRoute.RECEIPT_ROUTES.ADD)]
        public async Task<IActionResult> Add([FromBody] AddEditReceiptRequest request)
        {
            return await _receiptService.Add(User, request);
        }

        [Authorize]
        [HttpPut(ApiRoute.RECEIPT_ROUTES.EDIT)]
        public async Task<IActionResult> Edit([FromBody] AddEditReceiptRequest request, [FromRoute] int receiptId)
        {
            return await _receiptService.Edit(User, request, receiptId);

        }

        [Authorize]
        [HttpDelete(ApiRoute.RECEIPT_ROUTES.REMOVE)]
        public async Task<IActionResult> Remove([FromRoute] int receiptId)
        {
            return await _receiptService.Remove(User, receiptId);
        }

        [Authorize]
        [HttpPut(ApiRoute.RECEIPT_ROUTES.SET_SETTLED)]
        public async Task<IActionResult> SetSettled([FromRoute] int receiptId, [FromRoute] bool settled)
        {
            return await _receiptService.SetSettled(User, receiptId, settled);
        }

        [Authorize]
        [HttpGet(ApiRoute.RECEIPT_ROUTES.RECEIPT)]
        public async Task<IActionResult> GetReceipt([FromRoute] int receiptId)
        {
            return await _receiptService.GetReceipt(User, receiptId);
        }

        [Authorize]
        [HttpGet(ApiRoute.RECEIPT_ROUTES.RECEIPTS)]
        public async Task<IActionResult> GetReceipts()
        {
            return await _receiptService.GetReceipts(User);
        }

        [Authorize]
        [HttpGet(ApiRoute.RECEIPT_ROUTES.RECEIPTS_DATA_RANGE)]
        public async Task<IActionResult> GetReceiptsByDataRange([FromRoute] string from, [FromRoute] string to)
        {
            return await _receiptService.GetReceiptsByDataRange(User, from, to);
        }
    }
}
