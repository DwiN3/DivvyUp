using System.Runtime.InteropServices.JavaScript;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route("rm/person-product")]
    [ApiController]
    public class PersonProductController : ControllerBase
    {
        private readonly IPersonProductService _personProductService;

        public PersonProductController(IPersonProductService personProductService)
        {
            _personProductService = personProductService;
        }

        [Authorize]
        [HttpPost("{productId}/add")]
        public async Task<IActionResult> Add([FromBody] AddEditPersonProductRequest request, [FromRoute] int productId)
        {
            return await _personProductService.Add(User, request, productId);
        }

        [Authorize]
        [HttpPut("edit/{personProductId}")]
        public async Task<IActionResult> Edit([FromBody] AddEditPersonProductRequest request, [FromRoute] int personProductId)
        {
            return await _personProductService.Edit(User, request, personProductId);

        }

        [Authorize]
        [HttpDelete("remove/{personProductId}")]
        public async Task<IActionResult> Remove([FromRoute] int personProductId)
        {
            return await _personProductService.Remove(User, personProductId);
        }

        [Authorize]
        [HttpPut("{personProductId}/set-person")]
        public async Task<IActionResult> SetPerson([FromRoute] int personProductId, [FromQuery] int personId)
        {
            return await _personProductService.SetPerson(User, personProductId, personId);
        }

        [Authorize]
        [HttpPut("{personProductId}/settled")]
        public async Task<IActionResult> SetSettled([FromRoute] int personProductId, [FromQuery] bool settled)
        {
            return await _personProductService.SetSettled(User, personProductId, settled);
        }

        [Authorize]
        [HttpPut("{personProductId}/set-compensation")]
        public async Task<IActionResult> SetCompensation([FromRoute] int personProductId)
        {
            return await _personProductService.SetCompensation(User, personProductId);
        }

        [Authorize]
        [HttpGet("{personProductId}")]
        public async Task<IActionResult> GetPersonProduct([FromRoute] int personProductId)
        {
            return await _personProductService.GetPersonProduct(User, personProductId);
        }

        [Authorize]
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetPersonProductsFromProduct([FromRoute] int productId)
        {
            return await _personProductService.GetPersonProductsFromProduct(User, productId);
        }

        [Authorize]
        [HttpGet("person-products")]
        public async Task<IActionResult> GetPersonProducts()
        {
            return await _personProductService.GetPersonProducts(User);
        }
    }
}
