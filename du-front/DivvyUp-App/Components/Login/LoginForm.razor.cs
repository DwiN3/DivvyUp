using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.CodeReader;
using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Impl_Maui.Api.HttpResponseException;
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
        private DHttpClient DHttpClient { get; set; }
        [Inject]
        private UserAppService UserAppService { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }
        private CodeReaderResponse RCR { get; set; } = new();
        private UserDto User { get; set; } = new();

        private async Task SignUp()
        {
            try
            {
                var token = await AuthService.Login(User);
                UserDto user = await AuthService.GetUser(token);
                UserAppService.SetUser(user.username, user.email, token, true);
                DHttpClient.setToken(token);
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
