using System.Runtime.InteropServices.JavaScript;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route("rm/receipt/")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpPost]
        [Route("add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddEditReceiptRequest request)
        {
            return await _receiptService.Add(User, request);
        }

        [HttpPut]
        [Route("edit/{receiptId}")]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] AddEditReceiptRequest request, [FromRoute] int receiptId)
        {
            return await _receiptService.Edit(User, request, receiptId);

        }

        [HttpDelete]
        [Route("remove/{receiptId}")]
        [Authorize]
        public async Task<IActionResult> Remove([FromRoute] int receiptId)
        {
            return await _receiptService.Remove(User, receiptId);
        }

        [HttpPut]
        [Route("set-settled/{receiptId}/settled={settled}")]
        [Authorize]
        public async Task<IActionResult> SetSettled([FromRoute] int receiptId, [FromRoute] bool settled)
        {
            return await _receiptService.SetSettled(User, receiptId, settled);
        }

        [HttpGet]
        [Route("get/{receiptId}")]
        [Authorize]
        public async Task<IActionResult> GetReceipt([FromRoute] int receiptId)
        {
            return await _receiptService.GetReceipt(User, receiptId);
        }

        [HttpGet]
        [Route("get/receipts")]
        [Authorize]
        public async Task<IActionResult> GetReceipts()
        {
            return await _receiptService.GetReceipts(User);
        }

        [HttpGet]
        [Route("get/receipts-date-range/from={from}&to={to}")]
        [Authorize]
        public async Task<IActionResult> GetReceipts([FromRoute] DateTime from, [FromRoute] DateTime to)
        {
            return await _receiptService.GetReceiptsByDataRange(User, from, to);
        }
    }
}
