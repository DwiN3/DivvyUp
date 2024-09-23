using DivvyUp_Web.Api.Dtos;

namespace DivvyUp_Web.Api.Interface
{
    public interface IReceiptService
    {
        public Task AddReceipt(ReceiptDto receipt);
        public Task EditReceipt(ReceiptDto receipt);
        public Task SetSettled(int receiptId, bool isSettled);
        public Task Remove(int receiptId);
        public Task<List<ReceiptDto>> ShowAll();
    }
}
