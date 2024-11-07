using DivvyUp.Web.Interface;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DivvyUp.Web.Controllers
{
    [Authorize]
    [ApiController]
    [SwaggerTag("Product Management")]
    [Route(ApiRoute.PRODUCT_ROUTES.PRODUCT_ROUTE)]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost(ApiRoute.PRODUCT_ROUTES.ADD)]
        [SwaggerOperation(Summary = "Add a product to a receipt", Description = "Adds a new product to a specific receipt.")]
        public async Task<IActionResult> Add([FromBody] AddEditProductRequest request, [FromRoute] int receiptId)
        {
            return await _productService.Add(User, request , receiptId);
        }

        [HttpPut(ApiRoute.PRODUCT_ROUTES.EDIT)]
        [SwaggerOperation(Summary = "Edit a product", Description = "Edits the details of an existing product by its ID.")]
        public async Task<IActionResult> Edit([FromBody] AddEditProductRequest request, [FromRoute] int productId)
        {
            return await _productService.Edit(User, request, productId);

        }

        [HttpDelete(ApiRoute.PRODUCT_ROUTES.REMOVE)]
        [SwaggerOperation(Summary = "Remove a product", Description = "Removes a product by its ID.")]
        public async Task<IActionResult> Remove([FromRoute] int productId)
        {
            return await _productService.Remove(User, productId);
        }

        [HttpPut(ApiRoute.PRODUCT_ROUTES.SET_SETTLED)]
        [SwaggerOperation(Summary = "Mark product as settled", Description = "Marks a product as settled.")]
        public async Task<IActionResult> SetSettled([FromRoute] int productId, [FromRoute] bool settled)
        {
            return await _productService.SetSettled(User, productId, settled);
        }

        [HttpGet(ApiRoute.PRODUCT_ROUTES.PRODUCT)]
        [SwaggerOperation(Summary = "Retrieve a product", Description = "Retrieves the details of a product by its ID.")]
        public async Task<IActionResult> GetProduct([FromRoute] int productId)
        {
            return await _productService.GetProduct(User, productId);
        }

        [HttpGet(ApiRoute.PRODUCT_ROUTES.PRODUCTS)]
        [SwaggerOperation(Summary = "Retrieve a products", Description = "Retrieves all products associated with the current user.")]
        public async Task<IActionResult> GetProducts()
        {
            return await _productService.GetProducts(User);
        }

        [HttpGet(ApiRoute.PRODUCT_ROUTES.PRODUCTS_FROM_RECEIPT)]
        [SwaggerOperation(Summary = "Retrieve all products in a receipt", Description = "Retrieves all products associated with a specific receipt.")]
        public async Task<IActionResult> GetProductsFromReceipt([FromRoute] int receiptId)
        {
            return await _productService.GetProductsFromReceipt(User, receiptId);
        }
    }
}
