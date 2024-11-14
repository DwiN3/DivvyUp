using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;

namespace DivvyUp_Shared.Interfaces
{
    public interface IUserService
    {
        Task Register(RegisterUserDto request);
        Task<string> Login(LoginUserDto request);
        Task<string> Edit(EditUserDto request);
        Task Remove();
        Task<bool> ValidToken(string token);
        Task<UserDto> GetUser();
        Task ChangePassword(ChangePasswordUserDto request);
    }
}
