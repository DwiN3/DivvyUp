﻿using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Interfaces;
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
        public async Task<IActionResult> Add([FromBody] AddEditPersonProductDto request, [FromRoute] int productId)
        {
            await _personProductService.Add(request, productId);
            return Ok();
        }

        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.EDIT)]
        [SwaggerOperation(Summary = "Edit person-product association", Description = "Edits the details of an existing person-product association by its ID.")]
        public async Task<IActionResult> Edit([FromBody] AddEditPersonProductDto request, [FromRoute] int personProductId)
        {
            await _personProductService.Edit(request, personProductId);
            return Ok();
        }

        [HttpDelete(ApiRoute.PERSON_PRODUCT_ROUTES.REMOVE)]
        [SwaggerOperation(Summary = "Remove person-product association", Description = "Removes an association between a person and a product by its ID.")]
        public async Task<IActionResult> Remove([FromRoute] int personProductId)
        {
            await _personProductService.Remove(personProductId);
            return Ok();
        }

        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.SET_PERSON)]
        [SwaggerOperation(Summary = "Set person in person-product", Description = "Changes the person in an existing person-product association.")]
        public async Task<IActionResult> SetPerson([FromRoute] int personProductId, [FromRoute] int personId)
        {
            await _personProductService.SetPerson(personProductId, personId);
            return Ok();
        }

        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.SET_SETTLED)]
        [SwaggerOperation(Summary = "Set person-product as settled", Description = "Marks a person-product association as settled.")]
        public async Task<IActionResult> SetSettled([FromRoute] int personProductId, [FromRoute] bool settled)
        {
            await _personProductService.SetSettled(personProductId, settled);
            return Ok();
        }

        [HttpPut(ApiRoute.PERSON_PRODUCT_ROUTES.SET_COMPENSATION)]
        [SwaggerOperation(Summary = "Set person-product as compensation", Description = "Marks a person-product association as compensation.")]
        public async Task<IActionResult> SetCompensation([FromRoute] int personProductId)
        {
            await _personProductService.SetCompensation(personProductId);
            return Ok();
        }

        [HttpGet(ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCT)]
        [SwaggerOperation(Summary = "Retrieve person-product association", Description = "Retrieves the details of a person-product association by its ID.")]
        public async Task<IActionResult> GetPersonProduct([FromRoute] int personProductId)
        {
            var personProduct = await _personProductService.GetPersonProduct(personProductId);
            return Ok(personProduct);
        }

        [HttpGet(ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCT_FROM_PRODUCT)]
        [SwaggerOperation(Summary = "Retrieve all person-product associations for a product", Description = "Retrieves all person-product associations related to a specific product.")]
        public async Task<IActionResult> GetPersonProductsFromProduct([FromRoute] int productId)
        {
            var personProducts = await _personProductService.GetPersonProductsFromProduct(productId);
            return Ok(personProducts);
        }

        [HttpGet(ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCTS)]
        [SwaggerOperation(Summary = "Retrieve all person-product associations for user", Description = "Retrieves all person-product associations related to the logged-in user.")]
        public async Task<IActionResult> GetPersonProducts()
        {
            var personProducts = await _personProductService.GetPersonProducts();
            return Ok(personProducts);
        }
    }
}
