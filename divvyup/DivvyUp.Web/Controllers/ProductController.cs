using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Models;
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
        public async Task<IActionResult> Add([FromBody] AddEditProductDto request, [FromRoute] int receiptId)
        {
            var product = await _productService.Add(request, receiptId);
            return Ok(product);
        }

        [HttpPut(ApiRoute.PRODUCT_ROUTES.EDIT)]
        [SwaggerOperation(Summary = "Edit a product", Description = "Edits the details of an existing product by its ID.")]
        public async Task<IActionResult> Edit([FromBody] AddEditProductDto request, [FromRoute] int productId)
        {
            var product = await _productService.Edit(request, productId);
            return Ok(product);
        }

        [HttpPost(ApiRoute.PRODUCT_ROUTES.ADD_WIDTH_PERSON)]
        [SwaggerOperation(Summary = "Add a product and assign it to a person", Description = "Adds a product to a receipt and associates it with a specific person. This allows tracking who is responsible for the product.")]
        public async Task<IActionResult> AddWidthPerson([FromBody] AddEditProductDto request, [FromRoute] int receiptId, [FromRoute] int personId)
        {
            await _productService.AddWithPerson(request, receiptId, personId);
            return Ok();
        }

        [HttpPost(ApiRoute.PRODUCT_ROUTES.ADD_WIDTH_PERSONS)]
        [SwaggerOperation(Summary = "Add a product and assign it to a person", Description = "Adds a product to a receipt and associates it with a specific person. This allows tracking who is responsible for the product.")]
        public async Task<IActionResult> AddWidthPersons([FromBody] AddEditProductDto request, [FromRoute] int receiptId, [FromQuery] List<int> personIds)
        {
            await _productService.AddWithPersons(request, receiptId, personIds);
            return Ok();
        }

        [HttpPut(ApiRoute.PRODUCT_ROUTES.EDIT_WIDTH_PERSON)]
        [SwaggerOperation(Summary = "Edit a product and reassign it to a person", Description = "Edits the details of a product and reassigns it to a new person. This ensures the correct person is tracked for the product.")]
        public async Task<IActionResult> EditWithPerson([FromBody] AddEditProductDto request, [FromRoute] int productId, [FromRoute] int personId)
        {
            await _productService.EditWithPerson(request, productId, personId);
            return Ok();
        }

        [HttpDelete(ApiRoute.PRODUCT_ROUTES.REMOVE)]
        [SwaggerOperation(Summary = "Remove a product", Description = "Removes a product by its ID.")]
        public async Task<IActionResult> Remove([FromRoute] int productId)
        {
            await _productService.Remove(productId);
            return Ok();
        }

        [HttpPut(ApiRoute.PRODUCT_ROUTES.SET_SETTLED)]
        [SwaggerOperation(Summary = "Mark product as settled", Description = "Marks a product as settled.")]
        public async Task<IActionResult> SetSettled([FromRoute] int productId, [FromRoute] bool settled)
        {
            await _productService.SetSettled(productId, settled);
            return Ok();
        }

        [HttpGet(ApiRoute.PRODUCT_ROUTES.PRODUCT)]
        [SwaggerOperation(Summary = "Retrieve a product", Description = "Retrieves the details of a product by its ID.")]
        public async Task<IActionResult> GetProduct([FromRoute] int productId)
        {
            var product = await _productService.GetProduct(productId);
            return Ok(product);
        }

        [HttpGet(ApiRoute.PRODUCT_ROUTES.PRODUCTS)]
        [SwaggerOperation(Summary = "Retrieve a products", Description = "Retrieves all products associated with the current user.")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet(ApiRoute.PRODUCT_ROUTES.PRODUCTS_FROM_RECEIPT)]
        [SwaggerOperation(Summary = "Retrieve all products in a receipt", Description = "Retrieves all products associated with a specific receipt.")]
        public async Task<IActionResult> GetProductsFromReceipt([FromRoute] int receiptId)
        {
            var products = await _productService.GetProductsFromReceipt(receiptId);
            return Ok(products);
        }
    }
}
