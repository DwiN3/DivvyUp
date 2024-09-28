using Microsoft.AspNetCore.Components;
using DivvyUp_Impl_Maui.Api.DuHttpClient;
using DivvyUp_Impl_Maui.CodeReader;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;

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
            UserDto user = new UserDto
            {
                username = Username,
                password = Password
            };

            try
            {
                var token = await AuthService.Login(user);
                //LoginInfo = RCR.ReadLogin(response);
                User.SetUser(user.username, token, true);
                HttpClient.UpdateToken(token);
                ColorInfo = "green";
                Navigation.NavigateTo("/receipt");
            }
            catch (InvalidOperationException ex)
            {
                ColorInfo = "red";
            }
            
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Błąd połączenia z serwerem");
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
