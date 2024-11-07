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
    [SwaggerTag("Person Management")]
    [Route(ApiRoute.PERSON_ROUTES.PERSON_ROUTE)]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost(ApiRoute.PERSON_ROUTES.ADD)]
        [SwaggerOperation(Summary = "Add a new person", Description = "Adds a new person to the system.")]
        public async Task<IActionResult> Add([FromBody] AddEditPersonRequest request)
        {
            return await _personService.Add(User, request);
        }

        [HttpPut(ApiRoute.PERSON_ROUTES.EDIT)]
        [SwaggerOperation(Summary = "Edit a person", Description = "Edits the details of an existing person by their ID.")]
        public async Task<IActionResult> Edit([FromBody] AddEditPersonRequest request, [FromRoute] int personId)
        {
            return await _personService.Edit(User, request, personId);

        }

        [HttpDelete(ApiRoute.PERSON_ROUTES.REMOVE)]
        [SwaggerOperation(Summary = "Remove a person", Description = "Removes a person from the system using their ID.")]
        public async Task<IActionResult> Remove([FromRoute] int personId)
        {
            return await _personService.Remove(User, personId);
        }

        [HttpGet(ApiRoute.PERSON_ROUTES.PERSON)]
        [SwaggerOperation(Summary = "Retrieve person by ID", Description = "Retrieves the details of a person by their ID.")]
        public async Task<IActionResult> GetPerson([FromRoute] int personId)
        {
            return await _personService.GetPerson(User, personId);
        }

        [HttpGet(ApiRoute.PERSON_ROUTES.PEOPLE)]
        [SwaggerOperation(Summary = "Retrieve all persons", Description = "Retrieves all persons associated with the user's account.")]
        public async Task<IActionResult> GetPersons()
        {
            return await _personService.GetPersons(User);
        }

        [HttpGet(ApiRoute.PERSON_ROUTES.PERSON_USER)]
        [SwaggerOperation(Summary = "Retrieve person of logged-in user", Description = "Retrieves the person entity representing the currently logged-in user.")]
        public async Task<IActionResult> GetUserPerson()
        {
            return await _personService.GetUserPerson(User);
        }

        [HttpGet(ApiRoute.PERSON_ROUTES.PEOPLE_FROM_RECEIPT)]
        [SwaggerOperation(Summary = "Retrieve all persons from receipt", Description = "Retrieves all persons associated with a specific receipt by its ID.")]
        public async Task<IActionResult> GetPersonFromReceipt([FromRoute] int receiptId)
        {
            return await _personService.GetPersonFromReceipt(User, receiptId);
        }

        [HttpGet(ApiRoute.PERSON_ROUTES.PEOPLE_FROM_PRODUCT)]
        [SwaggerOperation(Summary = "Retrieve all persons from product", Description = "Retrieves all persons associated with a specific product by its ID.")]
        public async Task<IActionResult> GetPersonFromProduct([FromRoute] int productId)
        {
            return await _personService.GetPersonFromProduct(User, productId);
        }
    }
}
