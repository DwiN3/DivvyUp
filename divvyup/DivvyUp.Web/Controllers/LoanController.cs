using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DivvyUp.Web.Controllers
{
    [Authorize]
    [ApiController]
    [SwaggerTag("Loan Management")]
    [Route(ApiRoute.LOAN_ROUTES.LOAN_ROUTE)]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost(ApiRoute.LOAN_ROUTES.ADD)]
        [SwaggerOperation(Summary = "Add a new loan", Description = "Adds a new loan to the system.")]
        public async Task<IActionResult> Add([FromBody] AddEditLoanDto request)
        {
           await _loanService.Add(request);
           return Ok();
        }

        [HttpPut(ApiRoute.LOAN_ROUTES.EDIT)]
        [SwaggerOperation(Summary = "Edit a loan", Description = "Edits the details of an existing loan by its ID.")]
        public async Task<IActionResult> Edit([FromBody] AddEditLoanDto request, [FromRoute] int loanId)
        {
            await _loanService.Edit(request, loanId);
            return Ok();
        }

        [HttpDelete(ApiRoute.LOAN_ROUTES.REMOVE)]
        [SwaggerOperation(Summary = "Remove a loan", Description = "Removes a loan from the system by its ID.")]
        public async Task<IActionResult> Remove([FromRoute] int loanId)
        {
            await _loanService.Remove(loanId);
            return Ok();
        }

        [HttpPut(ApiRoute.LOAN_ROUTES.SET_PERSON)]
        [SwaggerOperation(Summary = "Set person in loan", Description = "Changes the person in an existing loan.")]
        public async Task<IActionResult> SetPerson([FromRoute] int loanId, [FromRoute] int personId)
        {
            await _loanService.SetPerson(loanId, personId);
            return Ok();
        }

        [HttpPut(ApiRoute.LOAN_ROUTES.SET_SETTLED)]
        [SwaggerOperation(Summary = "Set loan as settled", Description = "Marks a loan as settled by its ID.")]
        public async Task<IActionResult> SetSettled([FromRoute] int loanId, [FromRoute] bool settled)
        {
            await _loanService.SetSettled(loanId, settled);
            return Ok();
        }

        [HttpPut(ApiRoute.LOAN_ROUTES.SET_LENT)]
        [SwaggerOperation(Summary = "Set loan as lent", Description = "Marks a loan as lent by its ID.")]
        public async Task<IActionResult> SetLent([FromRoute] int loanId, [FromRoute] bool lent)
        {
            await _loanService.SetLent(loanId, lent);
            return Ok();
        }

        [HttpGet(ApiRoute.LOAN_ROUTES.LOAN)]
        [SwaggerOperation(Summary = "Retrieve a loan", Description = "Retrieves the details of a loan by its ID.")]
        public async Task<IActionResult> GetLoan([FromRoute] int loanId)
        {
            var loan = await _loanService.GetLoan(loanId);
            return Ok(loan);
        }

        [HttpGet(ApiRoute.LOAN_ROUTES.LOANS)]
        [SwaggerOperation(Summary = "Retrieve all loans", Description = "Retrieves all loans associated with the current user.")]
        public async Task<IActionResult> GetLoans()
        {
            var loans = await _loanService.GetLoans();
            return Ok(loans);
        }

        [HttpGet(ApiRoute.LOAN_ROUTES.LOANS_PERSON)]
        [SwaggerOperation(Summary = "Retrieve a loan by person", Description = "Retrieves the details of a loan associated with a specific person by their ID.")]
        public async Task<IActionResult> GetPersonLoans([FromRoute] int personId)
        {
            var loans = await _loanService.GetPersonLoans(personId);
            return Ok(loans);
        }

        [HttpGet(ApiRoute.LOAN_ROUTES.LOANS_DATA_RANGE)]
        [SwaggerOperation(Summary = "Retrieve all loans in date range", Description = "Retrieves all loans in date range associated with the current user.")]
        public async Task<IActionResult> GetLoansByDataRange([FromQuery] DateOnly from, [FromQuery] DateOnly to)
        {
            var loans = await _loanService.GetLoansByDataRange(from, to);
            return Ok(loans);
        }
    }
}
