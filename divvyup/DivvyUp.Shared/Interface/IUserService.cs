using DivvyUp_Shared.Dto;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp_Shared.Interface
{
    public interface IUserService
    {
        Task Register(RegisterRequest request);
        Task<string> Login(LoginRequest request);
        Task<string> Edit(RegisterRequest request);
        Task Remove();
        Task<bool> ValidToken(string token);
        Task<UserDto> GetUser();
        Task ChangePassword(ChangePasswordRequest request);
    }
}
