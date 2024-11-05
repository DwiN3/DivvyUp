using System.Runtime.InteropServices.JavaScript;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route(ApiRoute.LOAN_ROUTES.LOAN_ROUTE)]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [Authorize]
        [HttpPost(ApiRoute.LOAN_ROUTES.ADD)]
        public async Task<IActionResult> Add([FromBody] AddEditLoanRequest request)
        {
            return await _loanService.Add(User, request);
        }

        [Authorize]
        [HttpPut(ApiRoute.LOAN_ROUTES.EDIT)]
        public async Task<IActionResult> Edit([FromBody] AddEditLoanRequest request, [FromRoute] int loanId)
        {
            return await _loanService.Edit(User, request, loanId);
        }

        [Authorize]
        [HttpDelete(ApiRoute.LOAN_ROUTES.REMOVE)]
        public async Task<IActionResult> Remove([FromRoute] int loanId)
        {
            return await _loanService.Remove(User, loanId);
        }

        [Authorize]
        [HttpPut(ApiRoute.LOAN_ROUTES.SET_PERSON)]
        public async Task<IActionResult> SetPerson([FromRoute] int loanId, [FromRoute] int personId)
        {
            return await _loanService.SetPerson(User, loanId, personId);
        }

        [Authorize]
        [HttpPut(ApiRoute.LOAN_ROUTES.SET_SETTLED)]
        public async Task<IActionResult> SetSettled([FromRoute] int loanId, [FromRoute] bool settled)
        {
            return await _loanService.SetSettled(User, loanId, settled);
        }

        [Authorize]
        [HttpPut(ApiRoute.LOAN_ROUTES.SET_LENT)]
        public async Task<IActionResult> SetLent([FromRoute] int loanId, [FromRoute] bool lent)
        {
            return await _loanService.SetLent(User, loanId, lent);
        }

        [Authorize]
        [HttpGet(ApiRoute.LOAN_ROUTES.LOAN)]
        public async Task<IActionResult> GetLoan([FromRoute] int loanId)
        {
            return await _loanService.GetLoan(User, loanId);
        }

        [Authorize]
        [HttpGet(ApiRoute.LOAN_ROUTES.LOANS)]
        public async Task<IActionResult> GetLoans()
        {
            return await _loanService.GetLoans(User);
        }

        [Authorize]
        [HttpGet(ApiRoute.LOAN_ROUTES.LOANS_PERSON)]
        public async Task<IActionResult> GetPersonLoans([FromRoute] int personId)
        {
            return await _loanService.GetPersonLoans(User, personId);
        }

        [Authorize]
        [HttpGet(ApiRoute.LOAN_ROUTES.LOANS_DATA_RANGE)]
        public async Task<IActionResult> GetLoansByDataRange([FromRoute] string from, [FromRoute] string to)
        {
            return await _loanService.GetLoansByDataRange(User, from, to);
        }
    }
}
