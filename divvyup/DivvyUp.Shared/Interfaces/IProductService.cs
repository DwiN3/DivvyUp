using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;

namespace DivvyUp_Shared.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> Add(AddEditProductDto request, int receiptId);
        Task<ProductDto> Edit(AddEditProductDto request, int productId);
        Task Remove(int productId);
        Task SetSettled(int productId, bool settled);
        Task<ProductDto> GetProduct(int productId);
        Task<List<ProductDto>> GetProducts();
        Task<List<ProductDto>> GetProductsFromReceipt(int receiptId);
    }
}
