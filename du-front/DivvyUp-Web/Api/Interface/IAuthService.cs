using DivvyUp_Web.Api.Models;

namespace DivvyUp_Web.Api.Interface
{
    public interface IAuthService
    {
        public Task<HttpResponseMessage> Login(UserModel user);
        public Task<HttpResponseMessage> Register(UserModel user);
        public Task<HttpResponseMessage> RemoveAccount();
    }
}
