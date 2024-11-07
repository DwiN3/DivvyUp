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
    [SwaggerTag("Person Product Management")]
    [Route(ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCT_ROUTE)]
    public class PersonProductController : ControllerBase
    {
        private readonly IPersonProductService _personProductService;

        public PersonProductController(IPersonProductService personProductService)
        {
            _personProductService = personProductService;
        }

        [HttpPost(ApiRoute.PERSON_PRODUCT_ROUTES.ADD)]
        [SwaggerOperation(Summary = "Add person to product", Description = "Associates a person with a specific product.")]
        public async Task<IActionResult> Add([FromBody] AddEditPersonProductRequest request, [FromRoute] int productId)
        {
            return await _personProductService.Add(User, request, productId);
        }

        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.EDIT)]
        [SwaggerOperation(Summary = "Edit person-product association", Description = "Edits the details of an existing person-product association by its ID.")]
        public async Task<IActionResult> Edit([FromBody] AddEditPersonProductRequest request, [FromRoute] int personProductId)
        {
            return await _personProductService.Edit(User, request, personProductId);

        }

        [HttpDelete(ApiRoute.PERSON_PRODUCT_ROUTES.REMOVE)]
        [SwaggerOperation(Summary = "Remove person-product association", Description = "Removes an association between a person and a product by its ID.")]
        public async Task<IActionResult> Remove([FromRoute] int personProductId)
        {
            return await _personProductService.Remove(User, personProductId);
        }

        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.SET_PERSON)]
        [SwaggerOperation(Summary = "Set person in person-product", Description = "Changes the person in an existing person-product association.")]
        public async Task<IActionResult> SetPerson([FromRoute] int personProductId, [FromRoute] int personId)
        {
            return await _personProductService.SetPerson(User, personProductId, personId);
        }

        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.SET_SETTLED)]
        [SwaggerOperation(Summary = "Set person-product as settled", Description = "Marks a person-product association as settled.")]
        public async Task<IActionResult> SetSettled([FromRoute] int personProductId, [FromRoute] bool settled)
        {
            return await _personProductService.SetSettled(User, personProductId, settled);
        }

        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.SET_COMPENSATION)]
        [SwaggerOperation(Summary = "Set person-product as compensation", Description = "Marks a person-product association as compensation.")]
        public async Task<IActionResult> SetCompensation([FromRoute] int personProductId)
        {
            return await _personProductService.SetCompensation(User, personProductId);
        }

        [HttpGet(ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCT)]
        [SwaggerOperation(Summary = "Retrieve person-product association", Description = "Retrieves the details of a person-product association by its ID.")]
        public async Task<IActionResult> GetPersonProduct([FromRoute] int personProductId)
        {
            return await _personProductService.GetPersonProduct(User, personProductId);
        }

        [HttpGet(ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCT_FROM_PRODUCT)]
        [SwaggerOperation(Summary = "Retrieve all person-product associations for a product", Description = "Retrieves all person-product associations related to a specific product.")]
        public async Task<IActionResult> GetPersonProductsFromProduct([FromRoute] int productId)
        {
            return await _personProductService.GetPersonProductsFromProduct(User, productId);
        }

        [HttpGet(ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCTS)]
        [SwaggerOperation(Summary = "Retrieve all person-product associations for user", Description = "Retrieves all person-product associations related to the logged-in user.")]
        public async Task<IActionResult> GetPersonProducts()
        {
            return await _personProductService.GetPersonProducts(User);
        }
    }
}
