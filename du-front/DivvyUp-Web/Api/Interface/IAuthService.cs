using DivvyUp_Web.Api.Dtos;

namespace DivvyUp_Web.Api.Interface
{
    public interface IAuthService
    {
        public Task<HttpResponseMessage> Login(UserDto user);
        public Task<HttpResponseMessage> Register(UserDto user);
        public Task<HttpResponseMessage> RemoveAccount();
        public Task<HttpResponseMessage> isValid(string token);
    }
}
