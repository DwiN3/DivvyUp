using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IPersonService
    {
        Task Add(PersonDto person);
        Task Edit(PersonDto person);
        Task Remove(int personId);
        Task<PersonDto> GetPerson(int personId);
        Task<List<PersonDto>> GetPersons();
        Task<PersonDto> GetUserPerson();
        Task<List<PersonDto>> GetPersonFromReceipt(int receiptId);
        Task<List<PersonDto>> GetPersonFromProduct(int productId);
    }
}
