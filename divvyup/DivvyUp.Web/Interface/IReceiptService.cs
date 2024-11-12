using Microsoft.AspNetCore.Mvc;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp.Web.Interface
{
    public interface IReceiptService
    {
        Task Add(AddEditReceiptRequest request);
        Task Edit(AddEditReceiptRequest request, int receiptId);
        Task Remove(int receiptId);
        Task SetSettled(int receiptId, bool settled);
        Task<ReceiptDto> GetReceipt(int receiptId);
        Task<List<ReceiptDto>> GetReceipts();
        Task<List<ReceiptDto>> GetReceiptsByDataRange(string from, string to);
    }
}
