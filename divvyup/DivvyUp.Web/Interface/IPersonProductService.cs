using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DivvyUp_Shared.RequestDto;
using DivvyUp_Shared.Dto;

namespace DivvyUp.Web.Interface
{
    public interface IPersonProductService
    {
        Task Add(AddEditPersonProductRequest request, int productId);
        Task Edit(AddEditPersonProductRequest request, int personProductId);
        Task Remove(int personProductId);
        Task SetPerson(int personProductId, int personId);
        Task SetSettled(int personProductId, bool settled);
        Task SetCompensation(int personProductId);
        Task<PersonProductDto> GetPersonProduct(int personProductId);
        Task<List<PersonProductDto>> GetPersonProducts();
        Task<List<PersonProductDto>> GetPersonProductsFromProduct(int productId);
    }
}
