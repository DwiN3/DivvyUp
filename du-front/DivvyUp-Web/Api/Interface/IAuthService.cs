using DivvyUp_Web.Api.Models;

namespace DivvyUp_Web.Api.Interface
{
    public interface IAuthService
    {
        public Task<HttpResponseMessage> Login(User user);
        public Task<HttpResponseMessage> Register(User user);
        public Task<HttpResponseMessage> RemoveAccount();
    }
}
