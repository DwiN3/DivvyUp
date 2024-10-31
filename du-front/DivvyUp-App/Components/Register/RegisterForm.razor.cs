﻿using DivvyUp_App.GuiService;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;
using DivvyUp_Impl_Maui.Api.Exceptions;

namespace DivvyUp_App.Components.Register
{
    partial class RegisterForm
    {
        [Inject]
        private IUserHttpService UserService { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private UserDto User { get; set; } = new();
        private string RePassword { get; set; }


        private async Task CreateAccount()
        {
            if (User.password.Equals(RePassword))
            {
                try
                {
                    await UserService.Register(User);
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
