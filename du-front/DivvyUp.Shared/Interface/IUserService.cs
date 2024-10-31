
using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IUserService
    {
        Task<string> Login(UserDto user);
        Task Register(UserDto user);
        Task<string> Edit(UserDto user);
        Task Remove();
        Task ChangePassword(string password, string newPassword);
        Task<bool> IsValid(string token);
        Task<UserDto> GetUser();
    }
}
