using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IPersonProductService
    {
        Task AddProductPerson(PersonProductDto personProduct, int productId);
        Task RemoveProductPerson(int personProductId);
        Task SetSettledProductPerson(int personProductId, bool isSettled);
        Task SetCompensation(int personProductId, bool isCompensation);
        Task<PersonProductDto> GetPersonProduct(int personProductId);
        Task<List<PersonProductDto>> GetPersonProducts(int productId);
    }
}
