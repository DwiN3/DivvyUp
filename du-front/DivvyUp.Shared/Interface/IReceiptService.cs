using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IReceiptService
    {
        Task AddReceipt(ReceiptDto receipt);
        Task EditReceipt(ReceiptDto receipt);
        Task RemoveReceipt(int receiptId);
        Task SetSettledReceipt(int receiptId, bool isSettled);
        Task SetTotalPriceReceipt(int receiptId, double totalPrice);
        Task<ReceiptDto> GetReceipt(int receiptId);
        Task<List<ReceiptDto>> GetReceipts();
    }
}
