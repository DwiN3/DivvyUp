using System.Text;
using Newtonsoft.Json;
using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.Urls;
using DivvyUp_Web.Api.Models;

namespace DivvyUp_Web.Api.Service
{
    public class AuthService : IAuthService
    {
        private HttpClient _httpClient { get; set; } = new();
        private readonly Route _url;

        public AuthService(Route url)
        {
            _url = url;
        }

        public async Task<HttpResponseMessage> Login(User user)
        {
            var loginData = new
            {
                username = user.username,
                password = user.password
            };

            var jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_url.Login, content);
            return response;
        }

        public async Task<HttpResponseMessage> Register(User user)
        {
            var registerData = new
            {
                username = user.username,
                email = user.email,
                password = user.password
            };

            var jsonData = JsonConvert.SerializeObject(registerData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_url.Register, content);
            return response;
        }

        public async Task<HttpResponseMessage> RemoveAccount()
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> isValid(string token)
        {
            var url = $"{_url.IsValid}?token={token}";
            var response = await _httpClient.GetAsync(url);
            return response;
        }
    }
}
