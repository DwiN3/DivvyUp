using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receipt_Manager_Impl.Api.Interface
{
    public interface IAuthService
    {
        public Task<HttpResponseMessage> Register();
        public Task<HttpResponseMessage> Login(String username, String password);
        public Task<HttpResponseMessage> RemoveAccount();
    }
}
