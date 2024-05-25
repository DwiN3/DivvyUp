using System.Text;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.Urls;

namespace DivvyUp_Web.Api.Service
{
    public class AuthService : IAuthService
    {
        private Url _url { get; set; } = new();
        private HttpClient _http { get; set; } = new();

        public async Task<HttpResponseMessage> Login(string username, string password)
        {
            var loginData = new
            {
                username = username,
                password = password
            };

            var jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(_url.Login, content);
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
