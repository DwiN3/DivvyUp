using System.Runtime.InteropServices.JavaScript;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route(ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCT_ROUTE)]
    [ApiController]
    public class PersonProductController : ControllerBase
    {
        private readonly IPersonProductService _personProductService;

        public PersonProductController(IPersonProductService personProductService)
        {
            _personProductService = personProductService;
        }

        [Authorize]
        [HttpPost(ApiRoute.PERSON_PRODUCT_ROUTES.ADD)]
        public async Task<IActionResult> Add([FromBody] AddEditPersonProductRequest request, [FromRoute] int productId)
        {
            return await _personProductService.Add(User, request, productId);
        }

        [Authorize]
        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.EDIT)]
        public async Task<IActionResult> Edit([FromBody] AddEditPersonProductRequest request, [FromRoute] int personProductId)
        {
            return await _personProductService.Edit(User, request, personProductId);

        }

        [Authorize]
        [HttpDelete(ApiRoute.PERSON_PRODUCT_ROUTES.REMOVE)]
        public async Task<IActionResult> Remove([FromRoute] int personProductId)
        {
            return await _personProductService.Remove(User, personProductId);
        }

        [Authorize]
        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.SET_PERSON)]
        public async Task<IActionResult> SetPerson([FromRoute] int personProductId, [FromRoute] int personId)
        {
            return await _personProductService.SetPerson(User, personProductId, personId);
        }

        [Authorize]
        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.SET_SETTLED)]
        public async Task<IActionResult> SetSettled([FromRoute] int personProductId, [FromRoute] bool settled)
        {
            return await _personProductService.SetSettled(User, personProductId, settled);
        }

        [Authorize]
        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.SET_COMPENSATION)]
        public async Task<IActionResult> SetCompensation([FromRoute] int personProductId)
        {
            return await _personProductService.SetCompensation(User, personProductId);
        }

        [Authorize]
        [HttpGet(ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCT)]
        public async Task<IActionResult> GetPersonProduct([FromRoute] int personProductId)
        {
            return await _personProductService.GetPersonProduct(User, personProductId);
        }

        [Authorize]
        [HttpGet(ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCT_FROM_PRODUCT)]
        public async Task<IActionResult> GetPersonProductsFromProduct([FromRoute] int productId)
        {
            return await _personProductService.GetPersonProductsFromProduct(User, productId);
        }

        [Authorize]
        [HttpGet(ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCTS)]
        public async Task<IActionResult> GetPersonProducts()
        {
            return await _personProductService.GetPersonProducts(User);
        }
    }
}
