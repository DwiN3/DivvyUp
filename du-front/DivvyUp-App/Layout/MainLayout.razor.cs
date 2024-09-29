using DivvyUp_App.BaseComponents.DAlert;
using DivvyUp_Impl_Maui.Api.DuHttpClient;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DivvyUp_App.Layout
{
    partial class MainLayout : IDisposable
    {
        [Inject]
        private IAuthService AuthService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private DuHttpClient DuHttpClient { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }
        [Inject]
        private UserAppService UserAppService { get; set; }
        [Inject]
        private HeaderService HeaderService { get; set; } 
        private DAlert Alert { get; set; }

        private bool SidebarExpanded { get; set; } = false;
        private string Header { get; set; } = string.Empty;

        protected override async void OnInitialized()
        {
            Navigation.LocationChanged += OnLocationChanged;
            AlertService.OnAlert += ShowAlert;
            AlertService.OnCloseAlert += HideAlert;

            var user = UserAppService.GetUser();
            UserAppService.SetLoggedIn(false);

            if (!string.IsNullOrEmpty(user.token))
            {
                try
                {
                    bool isValid = await AuthService.IsValid(user.token);
                    if (isValid)
                    {
                        UserAppService.SetUser(user.username, user.token, true);
                        DuHttpClient.UpdateToken(user.token);
                        Navigation.NavigateTo("/receipt");
                    }
                }
                catch (HttpRequestException)
                {
                    UserAppService.ClearUser();
                    DuHttpClient.UpdateToken(string.Empty);
                    Navigation.NavigateTo("/");
                }
                catch (Exception)
                {
                    UserAppService.ClearUser();
                    DuHttpClient.UpdateToken(string.Empty);
                    Navigation.NavigateTo("/");
                }
            }
            SetHeader(Navigation.Uri);
            StateHasChanged();
        }

        private void OnLocationChanged(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            SetHeader(e.Location);
            StateHasChanged();
        }

        private void SetHeader(string url)
        {
            Header = HeaderService.GetHeader(url);
        }

        private async void ShowAlert(string message, AlertStyle style)
        {
            await Alert.OpenAlert(style, message);
        }

        private async void HideAlert()
        {
            await Alert.CloseAlert();
        }

        private async Task Logout()
        {
            UserAppService.ClearUser();
            StateHasChanged();
            Navigation.NavigateTo("/");
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
            AlertService.OnAlert -= ShowAlert;
            AlertService.OnCloseAlert -= HideAlert;
        }
    }
}
