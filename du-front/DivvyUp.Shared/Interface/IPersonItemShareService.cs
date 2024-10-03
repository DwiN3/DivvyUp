using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IPersonItemShareService
    {
        Task AddPersonItemShare(PersonItemShareDto personItemShare, int itemId);
        Task RemovePersonItemShare(int personItemShareId);
        Task SetPersonItemShare(int personItemShareId, bool isSettled);
        Task SetCompensationPersonItemShare(int personItemShareId);
        Task<PersonItemShareDto> GetPersonItemShare(int personItemShareId);
        Task<List<PersonItemShareDto>> GetPersonItemShares(int itemId);
    }
}
