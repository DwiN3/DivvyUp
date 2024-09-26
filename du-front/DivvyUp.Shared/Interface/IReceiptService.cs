using DivvyUp.Shared.Dto;

namespace DivvyUp.Shared.Interface
{
    public interface IReceiptService
    {
        public Task AddReceipt(ReceiptDto receipt);
        public Task EditReceipt(ReceiptDto receipt);
        public Task SetSettled(int receiptId, bool isSettled);
        public Task RemoveReceipt(int receiptId);
        public Task<List<ReceiptDto>> ShowAllReceipts();
    }
}
