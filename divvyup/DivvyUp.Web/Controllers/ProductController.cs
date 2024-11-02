using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route("rm/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpPost("{receiptId}/add")]
        public async Task<IActionResult> Add([FromBody] AddEditProductRequest request, [FromRoute] int receiptId)
        {
            return await _productService.Add(User, request , receiptId);
        }

        [Authorize]
        [HttpPut("edit/{productId}")]
        public async Task<IActionResult> Edit([FromBody] AddEditProductRequest request, [FromRoute] int productId)
        {
            return await _productService.Edit(User, request, productId);

        }

        [Authorize]
        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> Remove([FromRoute] int productId)
        {
            return await _productService.Remove(User, productId);
        }

        [Authorize]
        [HttpPut("{productId}/settled")]
        public async Task<IActionResult> SetSettled([FromRoute] int productId, [FromQuery] bool settled)
        {
            return await _productService.SetSettled(User, productId, settled);
        }

        [Authorize]
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct([FromRoute] int productId)
        {
            return await _productService.GetProduct(User, productId);
        }

        [Authorize]
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            return await _productService.GetProducts(User);
        }

        [Authorize]
        [HttpGet("products-from-receipt/{receiptId}")]
        public async Task<IActionResult> GetProductsFromReceipt([FromRoute] int receiptId)
        {
            return await _productService.GetProductsFromReceipt(User, receiptId);
        }
    }
}
