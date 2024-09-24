﻿using System.Net.Sockets;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.ResponceCodeReader;
using DivvyUp_Web.Api.Response;
using Blazored.LocalStorage;
using DivvyUp_Web.Api.Dtos;
using DivvyUp_Web.DuHttp;

namespace DivvyUp_App.Pages.Auth
{
    partial class Login
    {
        [Inject]
        private IAuthService AuthService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }
        [Inject]
        private DuHttpClient HttpClient { get; set; }

        private CodeReaderResponse RCR { get; set; } = new();
        private string Username { get; set; }
        private string Password { get; set; }
        private string LoginInfo { get; set; } = string.Empty;
        private string ColorInfo { get; set; } = "black";

        private async Task SingUp()
        {
            try
            {
                UserDto user = new UserDto();
                user.username = Username;
                user.password = Password;
                var response = await AuthService.Login(user);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody);
                    var token = loginResponse.token;

                    await LocalStorage.SetItemAsync("authToken", token);
                    await LocalStorage.SetItemAsync("username", user.username);
                    await LocalStorage.SetItemAsync("isLogin", true);
                    HttpClient.UpdateToken(token);
                    
                    ColorInfo = "green";
                    Navigation.NavigateTo("/receipt");
                }
                else
                {
                    ColorInfo = "red";
                }
                LoginInfo = RCR.ReadLogin(response.StatusCode);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException socketException)
            {
                Console.WriteLine($"Błąd połączenia: {socketException.Message}");
                LoginInfo = "Błąd połączenia z serwerem.";
                ColorInfo = "red";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                LoginInfo = "Wystąpił nieoczekiwany błąd.";
                ColorInfo = "red";
            }
        }
    }
}
