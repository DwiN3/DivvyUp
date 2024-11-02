using System.Runtime.InteropServices.JavaScript;
using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route("rm/loan")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddEditLoanRequest request)
        {
            return await _loanService.Add(User, request);
        }

        [Authorize]
        [HttpPut("edit/{loanId}")]
        public async Task<IActionResult> Edit([FromBody] AddEditLoanRequest request, [FromRoute] int loanId)
        {
            return await _loanService.Edit(User, request, loanId);

        }

        [Authorize]
        [HttpDelete("remove/{loanId}")]
        public async Task<IActionResult> Remove([FromRoute] int loanId)
        {
            return await _loanService.Remove(User, loanId);
        }

        [Authorize]
        [HttpPut("{loanId}/set-person")]
        public async Task<IActionResult> SetLent([FromRoute] int loanId, [FromQuery] int personId)
        {
            return await _loanService.SetPerson(User, loanId, personId);
        }

        [Authorize]
        [HttpPut("{loanId}/set-settled")]
        public async Task<IActionResult> SetSettled([FromRoute] int loanId, [FromQuery] bool settled)
        {
            return await _loanService.SetSettled(User, loanId, settled);
        }

        [Authorize]
        [HttpPut("{loanId}/set-lent")]
        public async Task<IActionResult> SetLent([FromRoute] int loanId, [FromQuery] bool lent)
        {
            return await _loanService.SetLent(User, loanId, lent);
        }

        [Authorize]
        [HttpGet("{loanId}")]
        public async Task<IActionResult> GetLoan([FromRoute] int loanId)
        {
            return await _loanService.GetLoan(User, loanId);
        }

        [Authorize]
        [HttpGet("loans")]
        public async Task<IActionResult> GetLoans()
        {
            return await _loanService.GetLoans(User);
        }

        [Authorize]
        [HttpGet("person-loans/{personId}")]
        public async Task<IActionResult> GetPersonLoan([FromRoute] int personId)
        {
            return await _loanService.GetPersonLoans(User, personId);
        }

        [Authorize]
        [HttpGet("date-range")]
        public async Task<IActionResult> GetLoansByDataRange([FromQuery] string from, [FromQuery] string to)
        {
            return await _loanService.GetLoansByDataRange(User, from, to);
        }
    }
}
