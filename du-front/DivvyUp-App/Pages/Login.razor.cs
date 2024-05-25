using System.Net.Sockets;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.ResponceCodeReader;
using DivvyUp_Web.Api.Response;
using Blazored.LocalStorage;

namespace DivvyUp_App.Pages
{
    partial class Login
    {
        private string Username { get; set; }
        private string Password { get; set; }
        [Inject]
        private IAuthService AuthService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }

        private ResponseCodeReader RCR { get; set; } = new();
        private string LoginInfo { get; set; } = string.Empty;
        private string ColorInfo { get; set; } = "black";
        

        private async Task SingUp()
        {
            try
            {
                var response = await AuthService.Login(Username, Password);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody);
                    await LocalStorage.SetItemAsync("authToken", loginResponse.token);
                    System.Diagnostics.Debug.Print(loginResponse.token);
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
