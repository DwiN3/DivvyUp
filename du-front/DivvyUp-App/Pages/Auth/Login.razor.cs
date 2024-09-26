using System.Net.Sockets;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using DivvyUp_Web.Api.Interface;
using Blazored.LocalStorage;
using DivvyUp_Impl.Interface;
using DivvyUp.Shared.Dto;
using DivvyUp_Impl.Api.DuHttpClient;
using DivvyUp_Impl.Api.Response;
using DivvyUp_Impl.CodeReader;
using DivvyUp_Impl.Service;

namespace DivvyUp_App.Pages.Auth
{
    partial class Login
    {
        [Inject]
        private IAuthService AuthService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private DuHttpClient HttpClient { get; set; }
        [Inject]
        private UserAppService User { get; set; }
        private CodeReaderResponse RCR { get; set; } = new();
        private string Username { get; set; }
        private string Password { get; set; }
        private string LoginInfo { get; set; } = string.Empty;
        private string ColorInfo { get; set; } = "black";

        private async Task SignUp()
        {
            try
            {
                UserDto user = new UserDto
                {
                    username = Username,
                    password = Password
                };
                var response = await AuthService.Login(user);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody);
                    var token = loginResponse.token;
                    User.SetUser(user.username, token, true);
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
