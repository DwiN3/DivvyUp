using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Impl_Maui.Api.HttpResponseException;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DivvyUp_App.Components.AccountManager
{
    partial class AccountManagerForm
    {
        [Inject]
        private IAuthService AuthService { get; set; }
        [Inject]
        private UserAppService UserAppService { get; set; }
        [Inject]
        private DHttpClient DHttpClient { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private DDialogService DDialogService { get; set; }
        private UserDto User { get; set; } = new();

        private string FileName { get; set; }
        private long? FileSize { get; set; }
        private string Avatar;

        protected override async Task OnInitializedAsync()
        {
            User.username = UserAppService.GetUser().username;
            User.email = UserAppService.GetUser().email;
        }

        private async Task SaveChanges()
        {
            try
            {
                var token = await AuthService.EditUser(User);
                DHttpClient.setToken(token);
                UserAppService.SetUser(User.username, User.email, token, true);
                AlertService.ShowAlert("Zapisano zmiany", AlertStyle.Success);
            }
            catch (HttpResponseException ex)
            {

            }
            catch (HttpRequestException)
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
            catch (HttpResponseException ex)
            {

            }
            catch (HttpRequestException)
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
                    await AuthService.RemoveUser();
                    UserAppService.ClearUser();
                    DHttpClient.setToken(string.Empty);
                    StateHasChanged();
                    Navigation.NavigateTo("/");
                }
            }
            catch (HttpResponseException ex)
            {

            }
            catch (HttpRequestException)
            {

            }
            catch (Exception)
            {
            }
        }
    }
}
