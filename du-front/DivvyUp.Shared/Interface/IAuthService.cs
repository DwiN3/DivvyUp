using DivvyUp.Shared.Dto;

namespace DivvyUp.Shared.Interface
{
    public interface IAuthService
    {
        public Task<HttpResponseMessage> Login(UserDto user);
        public Task<HttpResponseMessage> Register(UserDto user);
        public Task<HttpResponseMessage> RemoveAccount();
        public Task<HttpResponseMessage> isValid(string token);
    }
}
