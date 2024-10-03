using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IPersonProductService
    {
        Task AddPersonProduct(PersonProductDto personProduct, int productId);
        Task RemovePersonProduct(int personProductId);
        Task SetSettledPersonProduct(int personProductId, bool isSettled);
        Task SetCompensationPersonProduct(int personProductId);
        Task<PersonProductDto> GetPersonProduct(int personProductId);
        Task<List<PersonProductDto>> GetPersonProducts(int productId);
    }
}
