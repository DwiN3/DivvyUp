using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IChartService
    {
        Task<List<ChartDto>> GetTotalAmounts();
        Task<List<ChartDto>> GetUnpaindAmounts();
        Task<List<ChartDto>> GetPercantageExpenses();
        Task<List<ChartDto>> GetMonthlyTotalExpenses(int year);
        Task<List<ChartDto>> GetMonthlyUserExpenses(int year);
        Task<List<ChartDto>> GetWeeklyTotalExpenses();
        Task<List<ChartDto>> GetWeeklyUserExpenses();
        Task<List<ChartDto>> GetMonthlyTopProducts();
    }
}
