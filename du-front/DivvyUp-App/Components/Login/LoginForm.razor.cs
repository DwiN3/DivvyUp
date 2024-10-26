using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.Dto;
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
        private DNotificationService DNotificationService { get; set; }

        private UserDto User { get; set; } = new();

        private async Task SignUp()
        {
            try
            {
                var token = await UserService.Login(User);
                UserDto user = await UserService.GetUser(token);
                UserAppService.SetUser(user.username, user.email, token, true);
                DHttpClient.setToken(token);
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
