using System.Text;
using DivvyUp.Shared.Dto;
using Newtonsoft.Json;
using DivvyUp_Web.Api.Interface;
using AutoMapper;
using DivvyUp_Impl.Api.Route;

namespace DivvyUp_Impl.Service
{
    public class AuthService : IAuthService
    {
        private HttpClient _httpClient { get; set; } = new();
        private readonly Route _url;
        private readonly IMapper _mapper;

        public AuthService(Route url, IMapper mapper)
        {
            _url = url;
            _mapper = mapper;
        }

        public async Task<HttpResponseMessage> Login(UserDto user)
        {
            var loginData = new
            {
                user.username,
                user.password
            };

            var jsonData = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var url = _url.Login;
            var response = await _httpClient.PostAsync(url, content);
            return response;
        }

        public async Task<HttpResponseMessage> Register(UserDto user)
        {
            var registerData = new
            {
                user.username,
                user.email,
                user.password
            };

            var jsonData = JsonConvert.SerializeObject(registerData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var url = _url.Register;
            var response = await _httpClient.PostAsync(url, content);
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
