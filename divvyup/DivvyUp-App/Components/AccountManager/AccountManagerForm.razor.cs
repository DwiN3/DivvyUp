using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.HttpClients;
using DivvyUp_Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DivvyUp_App.Components.AccountManager
{
    partial class AccountManagerForm
    {
        [Inject]
        private IUserService UserService { get; set; }
        [Inject]
        private UserStateProvider UserStateProvider { get; set; }
        [Inject]
        private DHttpClient DHttpClient { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private DDialogService DDialogService { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }

        private EditUserDto EditData { get; set; } = new();
        private UserDto User { get; set; }
        private string FileName { get; set; }
        private long? FileSize { get; set; }
        private string Avatar;

        protected override async Task OnInitializedAsync()
        {
            User = await UserService.GetUser();
            SetUserData();
        }

        private void SetUserData()
        {
            EditData.Username = User.Username;
            EditData.Email = User.Email;
        }

        private async Task SaveChanges()
        {
            try
            {
                var token = await UserService.Edit(EditData);
                await UserStateProvider.SetTokenAsync(token);
                DNotificationService.ShowNotification("Zapisano zmiany", NotificationSeverity.Success);
            }
            catch (DException ex)
            {
                DNotificationService.ShowNotification(ex.Message, NotificationSeverity.Error);
                SetUserData();
            }
            catch (Exception)
            {
            }
        }

        private async Task ChangePassword()
        {
            try
            {
                await DDialogService.OpenResetPasswordDialog();
            }
            catch (DException ex)
            {
            }
            catch (Exception)
            {
            }
        }

        private async Task RemoveUser()
        {
            try
            {
                var result = await DDialogService.OpenYesNoDialog("Usuwanie konta", "Czy potwierdzasz usunięcie konta?");
                if (result)
                {
                    await UserService.Remove();
                    await UserStateProvider.ClearTokenAsync();
                    StateHasChanged();
                    Navigation.NavigateTo("/");
                }
            }
            catch (DException ex)
            {
            }
            catch (Exception)
            {
            }
        }
    }
}
