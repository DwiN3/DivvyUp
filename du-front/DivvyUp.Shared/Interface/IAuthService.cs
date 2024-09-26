using DivvyUp.Shared.Dto;
using DivvyUp.Shared.Response;

namespace DivvyUp.Shared.Interface
{
    public interface IAuthService
    {
        public Task<LoginResponse> Login(UserDto user);
        public Task Register(UserDto user);
        public Task RemoveAccount();
        public Task isValid(string token);
    }
}
