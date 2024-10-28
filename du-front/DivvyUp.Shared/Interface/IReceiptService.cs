using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IReceiptService
    {
        Task Add(ReceiptDto receipt);
        Task Edit(ReceiptDto receipt);
        Task Remove(int receiptId);
        Task SetSettled(int receiptId, bool isSettled);
        Task<ReceiptDto> GetReceipt(int receiptId);
        Task<List<ReceiptDto>> GetReceipts();
        Task<List<ReceiptDto>> GetReceiptsByDataRange(DateTime? from, DateTime? to);
    }
}
