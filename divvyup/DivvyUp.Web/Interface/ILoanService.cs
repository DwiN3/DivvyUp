using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp.Web.Interface
{
    public interface ILoanService
    {
        Task<IActionResult> Add(ClaimsPrincipal claims, AddEditLoanRequest request);
        Task<IActionResult> Edit(ClaimsPrincipal claims, AddEditLoanRequest request, int loanId);
        Task<IActionResult> Remove(ClaimsPrincipal claims, int loanId);
        Task<IActionResult> SetPerson(ClaimsPrincipal claims, int loanId, int personId);
        Task<IActionResult> SetSettled(ClaimsPrincipal claims, int loanId, bool settled);
        Task<IActionResult> SetLent(ClaimsPrincipal claims, int loanId, bool lent);
        Task<IActionResult> GetLoan(ClaimsPrincipal claims, int personId);
        Task<IActionResult> GetLoans(ClaimsPrincipal claims);
        Task<IActionResult> GetPersonLoans(ClaimsPrincipal claims, int personId);
        Task<IActionResult> GetLoansByDataRange(ClaimsPrincipal claims, string from, string to);
    }
}
