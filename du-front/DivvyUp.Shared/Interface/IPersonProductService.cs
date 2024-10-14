using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IPersonProductService
    {
        Task AddPersonProduct(PersonProductDto personProduct, int productId);
        Task EditPersonProduct(PersonProductDto personProduct);
        Task RemovePersonProduct(int personProductId);
        Task ChangePersonPersonProduct(int personProductId, int personId);
        Task SetSettledPersonProduct(int personProductId, bool isSettled);
        Task SetCompensationPersonProduct(int personProductId);
        Task<PersonProductDto> GetPersonProduct(int personProductId);
        Task<List<PersonProductDto>> GetPersonProductsFromProduct(int productId);
        Task<List<PersonProductDto>> GetPersonProducts();
    }
}
