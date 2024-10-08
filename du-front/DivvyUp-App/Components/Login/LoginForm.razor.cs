using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.DuHttpClient;
using DivvyUp_Impl_Maui.Api.HttpResponseException;
using DivvyUp_Impl_Maui.CodeReader;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DivvyUp_App.Components.Login
{
    partial class LoginForm
    {
        [Inject]
        private IAuthService AuthService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private DuHttpClient HttpClient { get; set; }
        [Inject]
        private UserAppService UserAppService { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }
        private CodeReaderResponse RCR { get; set; } = new();
        private string Username { get; set; }
        private string Password { get; set; }

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
                UserAppService.SetUser(user.username, token, true);
                HttpClient.UpdateToken(token);
                Navigation.NavigateTo("/receipt");
            }
            catch (HttpResponseException ex)
            {
                var message = RCR.ReadLogin(ex.StatusCode);
                AlertService.ShowAlert(message, AlertStyle.Danger);
            }
            catch (HttpRequestException)
            {
                AlertService.ShowAlert("Błąd połączenia z serwerem", AlertStyle.Warning);
            }
            catch (Exception)
            {
            }
        }
    }
}
