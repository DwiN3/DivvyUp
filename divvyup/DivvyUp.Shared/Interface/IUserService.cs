using DivvyUp_Shared.Dto;
using DivvyUp_Shared.RequestDto;

namespace DivvyUp_Shared.Interface
{
    public interface IUserService
    {
        Task Register(RegisterUserRequest request);
        Task<string> Login(LoginUserRequest request);
        Task<string> Edit(EditUserRequest request);
        Task Remove();
        Task<bool> ValidToken(string token);
        Task<UserDto> GetUser();
        Task ChangePassword(ChangePasswordUserRequest request);
    }
}
