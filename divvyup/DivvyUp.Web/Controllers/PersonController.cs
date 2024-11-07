using DivvyUp.Web.Interface;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route(ApiRoute.PERSON_ROUTES.PERSON_ROUTE)]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [Authorize]
        [HttpPost(ApiRoute.PERSON_ROUTES.ADD)]
        public async Task<IActionResult> Add([FromBody] AddEditPersonRequest request)
        {
            return await _personService.Add(User, request);
        }

        [Authorize]
        [HttpPut(ApiRoute.PERSON_ROUTES.EDIT)]
        public async Task<IActionResult> Edit([FromBody] AddEditPersonRequest request, [FromRoute] int personId)
        {
            return await _personService.Edit(User, request, personId);

        }

        [Authorize]
        [HttpDelete(ApiRoute.PERSON_ROUTES.REMOVE)]
        public async Task<IActionResult> Remove([FromRoute] int personId)
        {
            return await _personService.Remove(User, personId);
        }

        [Authorize]
        [HttpGet(ApiRoute.PERSON_ROUTES.PERSON)]
        public async Task<IActionResult> GetPerson([FromRoute] int personId)
        {
            return await _personService.GetPerson(User, personId);
        }

        [Authorize]
        [HttpGet(ApiRoute.PERSON_ROUTES.PEOPLE)]
        public async Task<IActionResult> GetPersons()
        {
            return await _personService.GetPersons(User);
        }

        [Authorize]
        [HttpGet(ApiRoute.PERSON_ROUTES.PERSON_USER)]
        public async Task<IActionResult> GetUserPerson()
        {
            return await _personService.GetUserPerson(User);
        }

        [Authorize]
        [HttpGet(ApiRoute.PERSON_ROUTES.PEOPLE_FROM_RECEIPT)]
        public async Task<IActionResult> GetPersonFromReceipt([FromRoute] int receiptId)
        {
            return await _personService.GetPersonFromReceipt(User, receiptId);
        }

        [Authorize]
        [HttpGet(ApiRoute.PERSON_ROUTES.PEOPLE_FROM_PRODUCT)]
        public async Task<IActionResult> GetPersonFromProduct([FromRoute] int productId)
        {
            return await _personService.GetPersonFromProduct(User, productId);
        }
    }
}
