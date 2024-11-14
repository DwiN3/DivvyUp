using DivvyUp_App.Service.Gui;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.RequestDto;

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

        private RegisterUserRequest RegisterData { get; set; } = new();
        private string RePassword { get; set; }


        private async Task CreateAccount()
        {
            if (RegisterData.Password.Equals(RePassword))
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
            else
            {
                DNotificationService.ShowNotification("Hasła są różne", NotificationSeverity.Error);
            }
        }
    }
}
