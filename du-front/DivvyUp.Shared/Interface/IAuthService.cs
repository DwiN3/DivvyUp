
using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IAuthService
    {
        Task<string> Login(UserDto user);
        Task Register(UserDto user);
        Task RemoveAccount();
        Task<bool> IsValid(string token);
    }
}
