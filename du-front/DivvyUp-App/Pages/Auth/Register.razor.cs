using System.Net.Sockets;
using DivvyUp.Shared.Dto;
using DivvyUp.Shared.Interface;
using DivvyUp_Impl.CodeReader;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Pages.Auth
{
    partial class Register
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
                UserDto user = new UserDto();
                user.username = Username;
                user.email = Email;
                user.password = Password;
                var response = await AuthService.Register(user);
                if(response.IsSuccessStatusCode)
                    ColorInfo = "green";
                else
                    ColorInfo = "red";
                
                RegisterInfo = RCR.ReadRegister(response.StatusCode);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException socketException)
            {
                RegisterInfo = "Błąd połączenia z serwerem.";
                ColorInfo = "red";
            }
            catch (Exception ex)
            {
                RegisterInfo = "Wystąpił nieoczekiwany błąd.";
                ColorInfo = "red";
            }
        }
    }
}
