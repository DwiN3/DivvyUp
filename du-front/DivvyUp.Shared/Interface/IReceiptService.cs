using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IReceiptService
    {
        Task AddReceipt(ReceiptDto receipt);
        Task EditReceipt(ReceiptDto receipt);
        Task RemoveReceipt(int receiptId);
        Task SetSettled(int receiptId, bool isSettled);
        Task<List<ReceiptDto>> ShowReceipts();
    }
}
