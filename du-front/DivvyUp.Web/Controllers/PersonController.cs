using DivvyUp.Web.Interface;
using DivvyUp.Web.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DivvyUp.Web.Controllers
{
    [Route("rm/person/")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IConfiguration _configuration;

        public PersonController(IPersonService personService, IConfiguration configuration)
        {
            _personService = personService;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("add")]
        [Authorize]
        public async Task<IActionResult> Add(AddEditPersonRequest request)
        {
            return await _personService.Add(request, User);
        }

        [HttpPut]
        [Route("edit")]
        [Authorize]
        public async Task<IActionResult> Edit(AddEditPersonRequest request, int personId)
        {
            return await _personService.Edit(request, personId, User);

        }

        [HttpDelete]
        [Route("remove/{personId}")]
        [Authorize]
        public async Task<IActionResult> Remove([FromQuery] int personId)
        {
            return await _personService.Remove(personId, User);
        }

        [HttpGet]
        [Route("get/{personId}")]
        [Authorize]
        public async Task<IActionResult> GetPerson(int personId)
        {
            return await _personService.GetPerson(personId, User);
        }

        [HttpGet]
        [Route("get/persons")]
        [Authorize]
        public async Task<IActionResult> GetPersons()
        {
            return await _personService.GetPersons(User);
        }

        [HttpGet]
        [Route("get/user-person")]
        [Authorize]
        public async Task<IActionResult> GetUserPerson()
        {
            return await _personService.GetUserPerson(User);
        }

        [HttpPost]
        [Route("get/{receiptId}/from-receipt")]
        [Authorize]
        public async Task<IActionResult> GetPersonFromReceipt([FromQuery] int receiptId)
        {
            return await _personService.GetPersonFromReceipt(receiptId, User);
        }

        [HttpPost]
        [Route("get/{productId}/from-product")]
        [Authorize]
        public async Task<IActionResult> GetPersonFromProduct([FromQuery] int productId)
        {
            return await _personService.GetPersonFromProduct(productId, User);
        }


    }
}
