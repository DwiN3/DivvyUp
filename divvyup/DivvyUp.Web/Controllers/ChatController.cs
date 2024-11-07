using DivvyUp.Web.Interfac;
using DivvyUp_Shared.AppConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DivvyUp.Web.Controllers
{
    [Authorize]
    [ApiController]
    [SwaggerTag("Chart")]
    [Route(ApiRoute.CHART_ROUTES.CHART_ROUTE)]
    public class ChartController : ControllerBase
    {
        private readonly IChartService _chartService;

        public ChartController(IChartService chartService)
        {
            _chartService = chartService;
        }

        [HttpGet(ApiRoute.CHART_ROUTES.TOTAL_AMOUNTS)]
        [SwaggerOperation(Summary = "Retrieve total amounts for chart", Description = "Fetches chart data showing the total costs related to people associated with the user's account.")]
        public async Task<IActionResult> GetTotalAmounts()
        {
            return await _chartService.GetAmounts(User, true);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.UNPAID_AMOUNTS)]
        [SwaggerOperation(Summary = "Retrieve unpaid amounts for chart", Description = "Fetches chart data showing the unpaid costs related to people associated with the user's account.")]
        public async Task<IActionResult> GetUnpaidAmounts()
        {
            return await _chartService.GetAmounts(User, false);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.PERCENTAGE_EXPENSES)]
        [SwaggerOperation(Summary = "Retrieve percentage of expenses paid", Description = "Fetches chart data showing the percentage of total costs paid by people associated with the user's account.")]
        public async Task<IActionResult> GetPercentageExpanses()
        {
            return await _chartService.GetPercentageExpanses(User);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.MONTHLY_TOTAL_EXPANSES)]
        [SwaggerOperation(Summary = "Retrieve total expenses by month", Description = "Fetches chart data showing the total expenses incurred by the user, grouped by each month. This data helps visualize monthly spending trends across all receipts and associated purchases")]
        public async Task<IActionResult> GetMonthlyTotalExpenses([FromRoute] int year)
        {
            return await _chartService.GetMonthlyTotalExpenses(User, year);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.MONTHLY_USER_EXPANSES)]
        [SwaggerOperation(Summary = "Retrieve total expenses for user by month", Description = "Fetches chart data showing the total expenses incurred by the user, grouped by each month. This data helps visualize monthly spending trends across all receipts and associated purchases")]
        public async Task<IActionResult> GetMonthlyUserExpenses([FromRoute] int year)
        {
            return await _chartService.GetMonthlyUserExpenses(User, year);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.WEEKLY_TOTAL_EXPENSES)]
        [SwaggerOperation(Summary = "Retrieve total expenses by week", Description = "Fetches chart data showing the total expenses incurred by the user, grouped by each week. This data helps visualize weekly spending trends across all receipts and associated purchases.")]
        public async Task<IActionResult> GetWeeklyTotalExpenses()
        {
            return await _chartService.GetWeeklyTotalExpenses(User);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.WEEKLY_USER_EXPENSES)]
        [SwaggerOperation(Summary = "Retrieve user expenses by week", Description = "Fetches chart data showing the user's expenses incurred by week. This data helps visualize the user's spending trends across all receipts and purchases.")]
        public async Task<IActionResult> GetWeeklyUserExpenses()
        {
            return await _chartService.GetWeeklyUserExpenses(User);
        }

        [HttpGet(ApiRoute.CHART_ROUTES.MONTHLY_TOP_PRODUCTS)]
        [SwaggerOperation(Summary = "Retrieve top 3 most expensive products for the current month", Description = "Fetches the three most expensive products purchased by the user in the current month.")]
        public async Task<IActionResult> GetMonthlyTopProducts()
        {
            return await _chartService.GetMonthlyTopProducts(User);
        }
    }
}
