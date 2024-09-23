using DivvyUp_Web.Api.Models;

namespace DivvyUp_Web.Api.Interface
{
    public interface IReceiptService
    {
        public Task<HttpResponseMessage> AddReceipt(Receipt receipt);
        public Task<HttpResponseMessage> EditReceipt(Receipt receipt);
        public Task<HttpResponseMessage> SetSettled(int receiptId, bool isSettled);
        public Task<HttpResponseMessage> Remove(int receiptId);
        public Task<HttpResponseMessage> ShowAll();
    }
}
