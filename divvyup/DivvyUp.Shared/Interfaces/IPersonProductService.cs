using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;

namespace DivvyUp_Shared.Interfaces
{
    public interface IPersonProductService
    {
        Task Add(AddEditPersonProductDto request, int productId);
        Task Edit(AddEditPersonProductDto request, int personProductId);
        Task Remove(int personProductId);
        Task RemoveList(int productId, List<int> personProductIds);
        Task SetPerson(int personProductId, int personId);
        Task SetSettled(int personProductId, bool settled);
        Task SetCompensation(int personProductId);
        Task<PersonProductDto> GetPersonProduct(int personProductId);
        Task<List<PersonProductDto>> GetPersonProductsFromPerson(int personId);
        Task<List<PersonProductDto>> GetPersonProducts();
        Task<List<PersonProductDto>> GetPersonProductsFromProduct(int productId);
    }
}
