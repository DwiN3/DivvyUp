
using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IAuthService
    {
        Task<string> Login(UserDto user);
        Task Register(UserDto user);
        Task<string> EditUser(UserDto user);
        Task ChangePassword(string password, string newPassword);
        Task RemoveUser();
        Task<bool> IsValid(string token);
        Task<UserDto> GetUser(string token);
    }
}
