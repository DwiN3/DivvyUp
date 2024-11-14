using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;

namespace DivvyUp_Shared.Interfaces
{
    public interface IPersonService
    {
        Task Add(AddEditPersonDto person);
        Task Edit(AddEditPersonDto person, int personId);
        Task Remove(int personId);
        Task<PersonDto> GetPerson(int personId);
        Task<List<PersonDto>> GetPersons();
        Task<PersonDto> GetUserPerson();
        Task<List<PersonDto>> GetPersonFromReceipt(int receiptId);
        Task<List<PersonDto>> GetPersonFromProduct(int productId);
    }
}
