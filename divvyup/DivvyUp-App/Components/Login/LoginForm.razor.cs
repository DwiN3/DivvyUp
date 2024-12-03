using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.HttpClients;
using DivvyUp_Shared.Interfaces;
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
        private UserStateProvider UserStateProvider { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }

        private LoginUserDto LoginData { get; set; } = new();

        private async Task SignUp()
        {
            try
            {
                var token = await UserService.Login(LoginData);
                await UserStateProvider.SetTokenAsync(token);
                Navigation.NavigateTo("/");
            }
            catch (DException ex)
            {
                DNotificationService.ShowNotification(ex.Message, NotificationSeverity.Error);
            }
            catch (TimeoutException)
            {
                DNotificationService.ShowNotification("Błąd połączenia z serwerem.", NotificationSeverity.Warning);
            }
            catch (Exception)
            {
            }
        }
    }
}
