using DivvyUp_Shared.Model;

namespace DivvyUp_Shared.Interface
{
    public interface IChartService
    {
        Task<List<ChartData>> GetTotalAmounts();
        Task<List<ChartData>> GetUnpaindAmounts();
        Task<List<ChartData>> GetPercantageExpenses();
    }
}
