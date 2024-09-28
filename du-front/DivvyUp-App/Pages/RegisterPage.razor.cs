using DivvyUp_Impl_Maui.CodeReader;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Pages
{
    partial class RegisterPage
    {
        [Inject]
        private IAuthService AuthService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        private CodeReaderResponse RCR { get; set; } = new();

        private string Username { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private string RegisterInfo { get; set; } = string.Empty;
        private string ColorInfo { get; set; } = "black";


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
                //RegisterInfo = RCR.ReadRegister(response.StatusCode);
                ColorInfo = "green";
            }
            catch (HttpRequestException ex)
            {
                ColorInfo = "red";
                RegisterInfo = "Błąd połączenia z serwerem.";
            }
            catch (Exception ex)
            {
                ColorInfo = "red";
            }
        }
    }
}
