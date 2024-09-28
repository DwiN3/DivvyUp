
using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IAuthService
    {
        public Task<string> Login(UserDto user);
        public Task Register(UserDto user);
        public Task RemoveAccount();
        public Task<bool> IsValid(string token);
    }
}
