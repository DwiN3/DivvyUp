using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp.Web.Interface
{
    public interface IProductService
    {
        Task<ProductDto> Add(AddEditProductRequest request, int receiptId);
        Task<ProductDto> Edit(AddEditProductRequest request, int productId);
        Task Remove(int productId);
        Task SetSettled(int productId, bool settled);
        Task<ProductDto> GetProduct(int productId);
        Task<List<ProductDto>> GetProducts();
        Task<List<ProductDto>> GetProductsFromReceipt(int receiptId);
    }
}
