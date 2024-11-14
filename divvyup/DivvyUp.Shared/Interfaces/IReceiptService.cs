using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;

namespace DivvyUp_Shared.Interfaces
{
    public interface IReceiptService
    {
        Task Add(AddEditReceiptDto request);
        Task Edit(AddEditReceiptDto request, int receiptId);
        Task Remove(int receiptId);
        Task SetSettled(int receiptId, bool settled);
        Task<ReceiptDto> GetReceipt(int receiptId);
        Task<List<ReceiptDto>> GetReceipts();
        Task<List<ReceiptDto>> GetReceiptsByDataRange(DateOnly from, DateOnly to);
    }
}
