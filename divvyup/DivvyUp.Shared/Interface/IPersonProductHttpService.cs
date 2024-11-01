using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IPersonProductHttpService
    {
        Task Add(PersonProductDto personProduct, int productId);
        Task Edit(PersonProductDto personProduct);
        Task Remove(int personProductId);
        Task SetPerson(int personProductId, int personId);
        Task SetSettled(int personProductId, bool isSettled);
        Task SetCompensation(int personProductId);
        Task<PersonProductDto> GetPersonProduct(int personProductId);
        Task<List<PersonProductDto>> GetPersonProductsFromProduct(int productId);
        Task<List<PersonProductDto>> GetPersonProducts();
    }
}
