using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IChartService
    {
        Task<List<ChartDto>> GetTotalAmounts();
        Task<List<ChartDto>> GetUnpaindAmounts();
        Task<List<ChartDto>> GetPercantageExpenses();
    }
}
