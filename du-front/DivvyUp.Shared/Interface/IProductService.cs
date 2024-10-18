using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IProductService
    {
        Task<ProductDto> Add(ProductDto product);
        Task<ProductDto> Edit(ProductDto product);
        Task Remove(int productId);
        Task SetSettled(int productId, bool isSettled);
        Task SetCompensationPrice(int productId, double compensationPrice);
        Task<ProductDto> GetProduct(int productId);
        Task<List<ProductDto>> GetProducts(int receiptId);
    }
}
