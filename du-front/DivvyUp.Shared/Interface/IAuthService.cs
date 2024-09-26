using DivvyUp.Shared.Dto;

namespace DivvyUp.Shared.Interface
{
    public interface IAuthService
    {
        public Task<string> Login(UserDto user);
        public Task Register(UserDto user);
        public Task RemoveAccount();
        public Task<bool> IsValid(string token);
    }
}
