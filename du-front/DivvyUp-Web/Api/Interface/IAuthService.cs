using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DivvyUp_Web.Api.Models;

namespace DivvyUp_Web.Api.Interface
{
    public interface IAuthService
    {
        public Task<HttpResponseMessage> Register(User user);
        public Task<HttpResponseMessage> Login(User user);
        public Task<HttpResponseMessage> RemoveAccount();
    }
}
