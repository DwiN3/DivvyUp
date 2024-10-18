using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IReceiptService
    {
        Task Add(ReceiptDto receipt);
        Task Edit(ReceiptDto receipt);
        Task Remove(int receiptId);
        Task SetSettled(int receiptId, bool isSettled);
        Task SetTotalPrice(int receiptId, double totalPrice);
        Task<ReceiptDto> GetReceipt(int receiptId);
        Task<List<ReceiptDto>> GetReceipts();
    }
}
