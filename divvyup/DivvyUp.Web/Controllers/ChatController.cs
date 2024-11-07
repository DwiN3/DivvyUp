using DivvyUp.Web.Interfac;
using DivvyUp_Shared.AppConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DivvyUp.Web.Controllers
{
    [Route(ApiRoute.CHART_ROUTES.CHART_ROUTE)]
    [ApiController]
    [Authorize]
    public class ChartController : ControllerBase
    {
        private readonly IChartService _chartService;

        public ChartController(IChartService chartService)
        {
            _chartService = chartService;
        }

        [HttpGet(ApiRoute.CHART_ROUTES.TOTAL_AMOUNTS)]
        public async Task<IActionResult> GetTotalAmounts()
        {
            return await _chartService.GetAmounts(User, true);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.UNPAID_AMOUNTS)]
        public async Task<IActionResult> GetUnpaidAmounts()
        {
            return await _chartService.GetAmounts(User, false);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.PERCENTAGE_EXPENSES)]
        public async Task<IActionResult> GetPercentageExpanses()
        {
            return await _chartService.GetPercentageExpanses(User);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.MONTHLY_TOTAL_EXPANSES)]
        public async Task<IActionResult> GetMonthlyTotalExpenses([FromRoute] int year)
        {
            return await _chartService.GetMonthlyTotalExpenses(User, year);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.MONTHLY_USER_EXPANSES)]
        public async Task<IActionResult> GetMonthlyUserExpenses([FromRoute] int year)
        {
            return await _chartService.GetMonthlyUserExpenses(User, year);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.WEEKLY_TOTAL_EXPENSES)]
        public async Task<IActionResult> GetWeeklyTotalExpenses()
        {
            return await _chartService.GetWeeklyTotalExpenses(User);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.WEEKLY_USER_EXPENSES)]
        public async Task<IActionResult> GetWeeklyUserExpenses()
        {
            return await _chartService.GetWeeklyUserExpenses(User);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.MONTHLY_TOP_PRODUCTS)]
        public async Task<IActionResult> GetMonthlyTopProducts()
        {
            return await _chartService.GetMonthlyTopProducts(User);
        }
    }
}
