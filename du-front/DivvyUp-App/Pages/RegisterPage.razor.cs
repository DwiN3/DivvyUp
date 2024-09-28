using DivvyUp_Impl_Maui.Api.HttpResponseException;
using DivvyUp_Impl_Maui.CodeReader;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DivvyUp_App.Pages
{
    partial class RegisterPage
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


        private async Task CreateAccount()
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
            }
            catch (HttpResponseException ex)
            {
                var message = RCR.ReadRegister(ex.StatusCode);
                AlertService.ShowAlert(message, AlertStyle.Danger);
            }
            catch (HttpRequestException ex)
            {
                AlertService.ShowAlert("Błąd połączenia z serwerem", AlertStyle.Warning);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
