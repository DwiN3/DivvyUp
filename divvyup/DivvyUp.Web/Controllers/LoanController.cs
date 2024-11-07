using System.Runtime.InteropServices.JavaScript;
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
        public async Task<IActionResult> Add([FromBody] AddEditLoanRequest request)
        {
            return await _loanService.Add(User, request);
        }

        [HttpPut(ApiRoute.LOAN_ROUTES.EDIT)]
        [SwaggerOperation(Summary = "Edit a loan", Description = "Edits the details of an existing loan by its ID.")]
        public async Task<IActionResult> Edit([FromBody] AddEditLoanRequest request, [FromRoute] int loanId)
        {
            return await _loanService.Edit(User, request, loanId);
        }

        [HttpDelete(ApiRoute.LOAN_ROUTES.REMOVE)]
        [SwaggerOperation(Summary = "Remove a loan", Description = "Removes a loan from the system by its ID.")]
        public async Task<IActionResult> Remove([FromRoute] int loanId)
        {
            return await _loanService.Remove(User, loanId);
        }

        [HttpPut(ApiRoute.LOAN_ROUTES.SET_PERSON)]
        [SwaggerOperation(Summary = "Set person in loan", Description = "Changes the person in an existing loan.")]
        public async Task<IActionResult> SetPerson([FromRoute] int loanId, [FromRoute] int personId)
        {
            return await _loanService.SetPerson(User, loanId, personId);
        }

        [HttpPut(ApiRoute.LOAN_ROUTES.SET_SETTLED)]
        [SwaggerOperation(Summary = "Set loan as settled", Description = "Marks a loan as settled by its ID.")]
        public async Task<IActionResult> SetSettled([FromRoute] int loanId, [FromRoute] bool settled)
        {
            return await _loanService.SetSettled(User, loanId, settled);
        }

        [HttpPut(ApiRoute.LOAN_ROUTES.SET_LENT)]
        [SwaggerOperation(Summary = "Set loan as lent", Description = "Marks a loan as lent by its ID.")]
        public async Task<IActionResult> SetLent([FromRoute] int loanId, [FromRoute] bool lent)
        {
            return await _loanService.SetLent(User, loanId, lent);
        }

        [HttpGet(ApiRoute.LOAN_ROUTES.LOAN)]
        [SwaggerOperation(Summary = "Retrieve a loan", Description = "Retrieves the details of a loan by its ID.")]
        public async Task<IActionResult> GetLoan([FromRoute] int loanId)
        {
            return await _loanService.GetLoan(User, loanId);
        }

        [HttpGet(ApiRoute.LOAN_ROUTES.LOANS)]
        [SwaggerOperation(Summary = "Retrieve all loans", Description = "Retrieves all loans associated with the current user.")]
        public async Task<IActionResult> GetLoans()
        {
            return await _loanService.GetLoans(User);
        }

        [HttpGet(ApiRoute.LOAN_ROUTES.LOANS_PERSON)]
        [SwaggerOperation(Summary = "Retrieve a loan by person", Description = "Retrieves the details of a loan associated with a specific person by their ID.")]
        public async Task<IActionResult> GetPersonLoans([FromRoute] int personId)
        {
            return await _loanService.GetPersonLoans(User, personId);
        }

        [HttpGet(ApiRoute.LOAN_ROUTES.LOANS_DATA_RANGE)]
        [SwaggerOperation(Summary = "Retrieve all loans in date range", Description = "Retrieves all loans in date range associated with the current user.")]
        public async Task<IActionResult> GetLoansByDataRange([FromRoute] string from, [FromRoute] string to)
        {
            return await _loanService.GetLoansByDataRange(User, from, to);
        }
    }
}
