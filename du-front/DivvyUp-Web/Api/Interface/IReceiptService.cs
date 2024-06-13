using DivvyUp_Web.Api.Models;

namespace DivvyUp_Web.Api.Interface
{
    public interface IReceiptService
    {
        public Task<HttpResponseMessage> AddReceipt(string token, ReceiptModel receipt);
        public Task<HttpResponseMessage> SetSettled(string token, int receiptId, bool isSettled);
        public Task<HttpResponseMessage> Remove(string token, int receiptId);
        public Task<HttpResponseMessage> ShowAll(string token);
    }
}
