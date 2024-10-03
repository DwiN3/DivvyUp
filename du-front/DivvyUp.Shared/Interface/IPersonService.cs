using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IPersonService
    {
        Task AddPerson(PersonDto person);
        Task EditPerson(PersonDto person);
        Task RemovePerson(int personId);
        Task SetReceiptsCountsPerson(int personId, int receiptsCounts);
        Task SetTotalPurchaseAmountPerson(int personId, double totalPurchaseAmount);
        Task<PersonDto> GetPerson(int personId);
        Task<List<PersonDto>> GetPersons();
    }
}
