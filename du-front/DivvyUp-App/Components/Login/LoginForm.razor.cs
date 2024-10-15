using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.CodeReader;
using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Impl_Maui.Api.HttpResponseException;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DivvyUp_App.Components.Login
{
    partial class LoginForm
    {
        [Inject]
        private IUserService UserService { get; set; }
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
                var token = await UserService.Login(User);
                UserDto user = await UserService.GetUser(token);
                UserAppService.SetUser(user.username, user.email, token, true);
                DHttpClient.setToken(token);
                Navigation.NavigateTo("/receipt");
            }
            catch (DuException ex)
            {
                var message = RCR.ReadLogin(ex.ErrorCode);
                AlertService.ShowAlert(message, AlertStyle.Danger);
            }
            catch (TimeoutException)
            {
                AlertService.ShowAlert("Błąd połączenia z serwerem.", AlertStyle.Warning);
            }
            catch (Exception)
            {
            }
        }
    }
}
