using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route("rm/person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddEditPersonRequest request)
        {
            return await _personService.Add(User, request);
        }

        [Authorize]
        [HttpPut("edit/{personId}")]
        public async Task<IActionResult> Edit([FromBody] AddEditPersonRequest request, [FromRoute] int personId)
        {
            return await _personService.Edit(User, request, personId);

        }

        [Authorize]
        [HttpDelete("remove/{personId}")]
        public async Task<IActionResult> Remove([FromRoute] int personId)
        {
            return await _personService.Remove(User, personId);
        }

        [Authorize]
        [HttpGet("{personId}")]
        public async Task<IActionResult> GetPerson([FromRoute] int personId)
        {
            return await _personService.GetPerson(User, personId);
        }

        [Authorize]
        [HttpGet("people")]
        public async Task<IActionResult> GetPersons()
        {
            return await _personService.GetPersons(User);
        }

        [Authorize]
        [HttpGet("user-person")]
        public async Task<IActionResult> GetUserPerson()
        {
            return await _personService.GetUserPerson(User);
        }

        [Authorize]
        [HttpGet("from-receipt/{receiptId}")]
        public async Task<IActionResult> GetPersonFromReceipt([FromRoute] int receiptId)
        {
            return await _personService.GetPersonFromReceipt(User, receiptId);
        }

        [Authorize]
        [HttpGet("from-product/{productId}")]
        public async Task<IActionResult> GetPersonFromProduct([FromRoute] int productId)
        {
            return await _personService.GetPersonFromProduct(User, productId);
        }
    }
}
