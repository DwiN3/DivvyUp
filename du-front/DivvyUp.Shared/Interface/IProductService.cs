using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IProductService
    {
        Task AddProduct(ProductDto product);
        Task EditProduct(ProductDto product);
        Task SetSettledProduct(int productId, bool isSettled);
        Task RemoveProduct(int productId);
        Task<ProductDto> ShowProduct(int productId);
        Task<List<ProductDto>> ShowProducts(int receiptID);
    }
}
