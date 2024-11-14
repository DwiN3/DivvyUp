using DivvyUp_Shared.Dtos.Entity;

namespace DivvyUp_Shared.Interfaces
{
    public interface IChartService
    {
        Task<List<ChartDto>> GetAmounts(bool isTotalAmounts);
        Task<List<ChartDto>> GetPercentageExpanses();
        Task<List<ChartDto>> GetMonthlyTotalExpenses(int year);
        Task<List<ChartDto>> GetMonthlyUserExpenses(int year);
        Task<List<ChartDto>> GetWeeklyTotalExpenses();
        Task<List<ChartDto>> GetWeeklyUserExpenses();
        Task<List<ChartDto>> GetMonthlyTopProducts();
    }
}
