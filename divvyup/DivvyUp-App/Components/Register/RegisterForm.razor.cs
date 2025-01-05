using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Dtos.Request;
using Microsoft.AspNetCore.Components;
using Radzen;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using System.Text.Json;

namespace DivvyUp_App.Components.Register
{
    partial class RegisterForm
    {
        [Inject]
        private IUserService UserService { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private RegisterUserDto RegisterData { get; set; } = new();
        private string RePassword { get; set; }


        private async Task CreateAccount()
        {
            try
            {
                await UserService.Register(RegisterData);
                DNotificationService.ShowNotification("Pomyślnie utworzono użytkownika", NotificationSeverity.Success);
                Navigation.NavigateTo("/login");
            }
            catch (DException ex)
            {
                DNotificationService.ShowNotification(ex.Message, NotificationSeverity.Error);
            }
            catch (TimeoutException)
            {
                DNotificationService.ShowNotification("Błąd połączenia z serwerem", NotificationSeverity.Warning);
            }
            catch (Exception)
            {
            }
        }

        void OnSubmit(RegisterUserDto model)
        {
            CreateAccount();
        }

        void OnInvalidSubmit(FormInvalidSubmitEventArgs args)
        {
            
        }

        bool ValidateOneUpperLetter(string password)
        {
            return password.Any(char.IsUpper);
        }
        bool ValidateOneSpecialCharacter(string password)
        {
            const string specialCharacters = "!@#$%^&*()-_=+[]{}|;:'\",.<>?/`~";
            return password.Any(ch => specialCharacters.Contains(ch));
        }
        bool ValidateOneNumber(string password)
        {
            const string numbers = "0123456789";
            return password.Any(ch => numbers.Contains(ch));
        }
    }
}
