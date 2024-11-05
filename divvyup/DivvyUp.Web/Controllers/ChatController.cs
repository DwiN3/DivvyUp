using DivvyUp.Web.InterfaceWeb;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route("rm/chart")]
    [ApiController]
    [Authorize]
    public class ChartController : ControllerBase
    {
        private readonly IChartService _chartService;

        public ChartController(IChartService chartService)
        {
            _chartService = chartService;
        }

        [HttpGet("total-amounts")]
        public async Task<IActionResult> GetTotalAmounts()
        {
            return await _chartService.GetAmounts(User, true);
        }

        [HttpGet("unpaid-amounts")]
        public async Task<IActionResult> GetUnpaidAmounts()
        {
            return await _chartService.GetAmounts(User, false);
        }

        [HttpGet("percentage-expenses")]
        public async Task<IActionResult> GetPercentageExpanses()
        {
            return await _chartService.GetPercentageExpanses(User);
        }

        [HttpGet("monthly-total-expenses")]
        public async Task<IActionResult> GetMonthlyTotalExpenses([FromQuery] int year)
        {
            return await _chartService.GetMonthlyTotalExpenses(User, year);
        }

        [HttpGet("monthly-user-expenses")]
        public async Task<IActionResult> GetMonthlyUserExpenses([FromQuery] int year)
        {
            return await _chartService.GetMonthlyUserExpenses(User, year);
        }

        [HttpGet("weekly-total-expenses")]
        public async Task<IActionResult> GetWeeklyTotalExpenses()
        {
            return await _chartService.GetWeeklyTotalExpenses(User);
        }

        [HttpGet("weekly-user-expenses")]
        public async Task<IActionResult> GetWeeklyUserExpenses()
        {
            return await _chartService.GetWeeklyUserExpenses(User);
        }

        [HttpGet("monthly-top-products")]
        public async Task<IActionResult> GetMonthlyTopProducts()
        {
            return await _chartService.GetMonthlyTopProducts(User);
        }
    }
}
