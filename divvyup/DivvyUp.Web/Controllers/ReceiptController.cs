using System.Runtime.InteropServices.JavaScript;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route("rm/receipt")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddEditReceiptRequest request)
        {
            return await _receiptService.Add(User, request);
        }

        [Authorize]
        [HttpPut("edit/{receiptId}")]
        public async Task<IActionResult> Edit([FromBody] AddEditReceiptRequest request, [FromRoute] int receiptId)
        {
            return await _receiptService.Edit(User, request, receiptId);

        }

        [Authorize]
        [HttpDelete("remove/{receiptId}")]
        public async Task<IActionResult> Remove([FromRoute] int receiptId)
        {
            return await _receiptService.Remove(User, receiptId);
        }

        [Authorize]
        [HttpPut("{receiptId}/settled")]
        public async Task<IActionResult> SetSettled([FromRoute] int receiptId, [FromQuery] bool settled)
        {
            return await _receiptService.SetSettled(User, receiptId, settled);
        }

        [Authorize]
        [HttpGet("{receiptId}")]
        public async Task<IActionResult> GetReceipt([FromRoute] int receiptId)
        {
            return await _receiptService.GetReceipt(User, receiptId);
        }

        [Authorize]
        [HttpGet("receipts")]
        public async Task<IActionResult> GetReceipts()
        {
            return await _receiptService.GetReceipts(User);
        }

        [Authorize]
        [HttpGet("date-range")]
        public async Task<IActionResult> GetReceiptsByDataRange([FromQuery] string from, [FromQuery] string to)
        {
            return await _receiptService.GetReceiptsByDataRange(User, from, to);
        }
    }
}
