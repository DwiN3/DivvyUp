using DivvyUp_Impl_Maui.CodeReader;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using DivvyUp_Impl_Maui.Api.HttpResponseException;
using Radzen;

namespace DivvyUp_App.Components.Register
{
    partial class RegisterForm
    {
        [Inject]
        private IAuthService AuthService { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }
        private CodeReaderResponse RCR { get; set; } = new();

        private string Username { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private string RePassword { get; set; }


        private async Task CreateAccount()
        {
            if (Password.Equals(RePassword))
            {
                try
                {
                    UserDto user = new UserDto
                    {
                        username = Username,
                        email = Email,
                        password = Password
                    };
                    await AuthService.Register(user);
                    AlertService.ShowAlert("Pomyślnie utworzono użytkownika", AlertStyle.Success);
                    Navigation.NavigateTo("/login");
                }
                catch (HttpResponseException ex)
                {
                    var message = RCR.ReadRegister(ex.StatusCode);
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
            else
            {
                AlertService.ShowAlert("Hasła są różne", AlertStyle.Danger);
            }
        }
    }
}
