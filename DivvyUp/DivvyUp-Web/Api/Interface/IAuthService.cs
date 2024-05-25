using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivvyUp_Web.Api.Interface
{
    public interface IAuthService
    {
        public Task<HttpResponseMessage> Register();
        public Task<HttpResponseMessage> Login(string username, string password);
        public Task<HttpResponseMessage> RemoveAccount();
    }
}
