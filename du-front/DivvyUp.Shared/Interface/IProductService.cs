using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IProductService
    {
        Task AddProduct(ProductDto product);
        Task EditProduct(ProductDto product);
        Task RemoveProduct(int productId);
        Task SetSettledProduct(int productId, bool isSettled);
        Task SetCompensationPriceProduct(int productId, double compensationPrice);
        Task<ProductDto> GetProduct(int productId);
        Task<List<ProductDto>> GetProducts(int receiptId);
    }
}
