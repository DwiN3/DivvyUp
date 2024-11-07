using DivvyUp.Web.Interface;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route(ApiRoute.PRODUCT_ROUTES.PRODUCT_ROUTE)]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpPost(ApiRoute.PRODUCT_ROUTES.ADD)]
        public async Task<IActionResult> Add([FromBody] AddEditProductRequest request, [FromRoute] int receiptId)
        {
            return await _productService.Add(User, request , receiptId);
        }

        [Authorize]
        [HttpPut(ApiRoute.PRODUCT_ROUTES.EDIT)]
        public async Task<IActionResult> Edit([FromBody] AddEditProductRequest request, [FromRoute] int productId)
        {
            return await _productService.Edit(User, request, productId);

        }

        [Authorize]
        [HttpDelete(ApiRoute.PRODUCT_ROUTES.REMOVE)]
        public async Task<IActionResult> Remove([FromRoute] int productId)
        {
            return await _productService.Remove(User, productId);
        }

        [Authorize]
        [HttpPut(ApiRoute.PRODUCT_ROUTES.SET_SETTLED)]
        public async Task<IActionResult> SetSettled([FromRoute] int productId, [FromRoute] bool settled)
        {
            return await _productService.SetSettled(User, productId, settled);
        }

        [Authorize]
        [HttpGet(ApiRoute.PRODUCT_ROUTES.PRODUCT)]
        public async Task<IActionResult> GetProduct([FromRoute] int productId)
        {
            return await _productService.GetProduct(User, productId);
        }

        [Authorize]
        [HttpGet(ApiRoute.PRODUCT_ROUTES.PRODUCTS)]
        public async Task<IActionResult> GetProducts()
        {
            return await _productService.GetProducts(User);
        }

        [Authorize]
        [HttpGet(ApiRoute.PRODUCT_ROUTES.PRODUCTS_FROM_RECEIPT)]
        public async Task<IActionResult> GetProductsFromReceipt([FromRoute] int receiptId)
        {
            return await _productService.GetProductsFromReceipt(User, receiptId);
        }
    }
}
