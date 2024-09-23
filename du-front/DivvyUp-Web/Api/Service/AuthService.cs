﻿using System.Text;
using DivvyUp_Web.Api.Dtos;
using Newtonsoft.Json;
using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.Urls;
using AutoMapper;

namespace DivvyUp_Web.Api.Service
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
                username = user.username,
                password = user.password
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
                username = user.username,
                email = user.email,
                password = user.password
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
