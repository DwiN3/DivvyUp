using DivvyUp_App.GuiService;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;
using DivvyUp_Impl_Maui.Api.HttpResponseException;
using DivvyUp_Impl_Maui.Api.CodeReader;
using DivvyUp_Shared.Exceptions;

namespace DivvyUp_App.Components.Register
{
    partial class RegisterForm
    {
        [Inject]
        private IUserService UserService { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }
        private CodeReaderResponse RCR { get; set; } = new();

        private UserDto User { get; set; } = new();
        private string RePassword { get; set; }


        private async Task CreateAccount()
        {
            if (User.password.Equals(RePassword))
            {
                try
                {
                    await UserService.Register(User);
                    AlertService.ShowAlert("Pomyślnie utworzono użytkownika", AlertStyle.Success);
                    Navigation.NavigateTo("/login");
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
            else
            {
                AlertService.ShowAlert("Hasła są różne", AlertStyle.Danger);
            }
        }
    }
}
