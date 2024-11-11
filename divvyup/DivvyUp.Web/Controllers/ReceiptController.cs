using DivvyUp.Web.Interface;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DivvyUp.Web.Controllers
{
    [Authorize]
    [ApiController]
    [SwaggerTag("Receipt Management")]
    [Route(ApiRoute.RECEIPT_ROUTES.RECEIPT_ROUTE)]
    public class ReceiptController : ControllerBase, IReceiptService
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpPost(ApiRoute.RECEIPT_ROUTES.ADD)]
        [SwaggerOperation(Summary = "Add a new receipt", Description = "Adds a new receipt to the system.")]
        public async Task<IActionResult> Add([FromBody] AddEditReceiptRequest request)
        {
            return await _receiptService.Add(request);
        }

        [HttpPut(ApiRoute.RECEIPT_ROUTES.EDIT)]
        [SwaggerOperation(Summary = "Edit a receipt", Description = "Edits the details of an existing receipt by its ID.")]
        public async Task<IActionResult> Edit([FromBody] AddEditReceiptRequest request, [FromRoute] int receiptId)
        {
            return await _receiptService.Edit(request, receiptId);

        }

        [HttpDelete(ApiRoute.RECEIPT_ROUTES.REMOVE)]
        [SwaggerOperation(Summary = "Remove a receipt", Description = "Removes a receipt from the system by its ID.")]
        public async Task<IActionResult> Remove([FromRoute] int receiptId)
        {
            return await _receiptService.Remove(receiptId);
        }

        [HttpPut(ApiRoute.RECEIPT_ROUTES.SET_SETTLED)]
        [SwaggerOperation(Summary = "Set receipt as settled", Description = "Marks a receipt as settled by its ID.")]
        public async Task<IActionResult> SetSettled([FromRoute] int receiptId, [FromRoute] bool settled)
        {
            return await _receiptService.SetSettled(receiptId, settled);
        }

        [HttpGet(ApiRoute.RECEIPT_ROUTES.RECEIPT)]
        [SwaggerOperation(Summary = "Retrieve a receipt", Description = "Retrieves the details of a receipt by its ID.")]
        public async Task<ActionResult<ReceiptDto>> GetReceipt([FromRoute] int receiptId)
        {
            return await _receiptService.GetReceipt(receiptId);
        }

        [HttpGet(ApiRoute.RECEIPT_ROUTES.RECEIPTS)]
        [SwaggerOperation(Summary = "Retrieve all receipts", Description = "Retrieves all receipts associated with the current user.")]
        public async Task<ActionResult<List<ReceiptDto>>> GetReceipts()
        {
            return await _receiptService.GetReceipts();
        }

        [HttpGet(ApiRoute.RECEIPT_ROUTES.RECEIPTS_DATA_RANGE)]
        [SwaggerOperation(Summary = "Retrieve all receipts in date range", Description = "Retrieves all receipts in date range associated with the current user.")]
        public async Task<ActionResult<List<ReceiptDto>>> GetReceiptsByDataRange([FromRoute] string from, [FromRoute] string to)
        {
            return await _receiptService.GetReceiptsByDataRange(from, to);
        }
    }
}
