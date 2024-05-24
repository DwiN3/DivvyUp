using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Receipt_Manager_Impl.Api.Interface;
using Receipt_Manager_Impl.Api.Models;
using Receipt_Manager_Impl.Api.Url;

namespace Receipt_Manager_Impl.Api.Service
{
    public class AuthService : IAuthService
    { 
        private HttpClient http = new HttpClient();

        private AuthUrl url = new AuthUrl();

        public async Task<HttpResponseMessage> Login(String username, String password)
        {
            var loginData = new
            {
                username = username,
                password = password
            };

            var jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await http.PostAsync(url.Login, content);
            return response;
        }

        public Task<HttpResponseMessage> Register()
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> RemoveAccount()
        {
            throw new NotImplementedException();
        }
    }
}
