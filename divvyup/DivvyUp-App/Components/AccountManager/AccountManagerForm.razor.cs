using DivvyUp_App.Service.Gui;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.HttpClients;
using DivvyUp_Shared.Interface;
using DivvyUp_Shared.RequestDto;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DivvyUp_App.Components.AccountManager
{
    partial class AccountManagerForm
    {
        [Inject]
        private IUserService UserService { get; set; }
        [Inject]
        private UserAppService UserAppService { get; set; }
        [Inject]
        private DHttpClient DHttpClient { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private DDialogService DDialogService { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }

        private EditUserRequest EditData { get; set; } = new();
        private string FileName { get; set; }
        private long? FileSize { get; set; }
        private string Avatar;

        protected override async Task OnInitializedAsync()
        {
            EditData.Username = UserAppService.GetUser().username;
            EditData.Email = UserAppService.GetUser().email;
        }

        private async Task SaveChanges()
        {
            try
            {
                var token = await UserService.Edit(EditData);
                DHttpClient.setToken(token);
                UserAppService.SetUser(EditData.Username, EditData.Email, token, true);
                DNotificationService.ShowNotification("Zapisano zmiany", NotificationSeverity.Success);
            }
            catch (DException ex)
            {
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
                    UserAppService.ClearUser();
                    DHttpClient.setToken(string.Empty);
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
