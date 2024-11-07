using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DivvyUp.Web.Interfac
{
    public interface IChartService
    {
        Task<IActionResult> GetAmounts(ClaimsPrincipal claims, bool isTotalAmounts);
        Task<IActionResult> GetPercentageExpanses(ClaimsPrincipal claims);
        Task<IActionResult> GetMonthlyTotalExpenses(ClaimsPrincipal claims, int year);
        Task<IActionResult> GetMonthlyUserExpenses(ClaimsPrincipal claims, int year);
        Task<IActionResult> GetWeeklyTotalExpenses(ClaimsPrincipal claims);
        Task<IActionResult> GetWeeklyUserExpenses(ClaimsPrincipal claims);
        Task<IActionResult> GetMonthlyTopProducts(ClaimsPrincipal claims);
    }
}
