using DivvyUp_App.BaseComponents.DAlert;
using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DivvyUp_App.Layout
{
    partial class MainLayout : IDisposable
    {
        [Inject]
        private IUserService UserService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private DHttpClient DHttpClient { get; set; }
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
            await SetUser();
            SetHeader(Navigation.Uri);
            StateHasChanged();
        }

        private async Task SetUser()
        {
            var user = UserAppService.GetUser();
            UserAppService.SetLoggedIn(false);

            if (!string.IsNullOrEmpty(user.token))
            {
                try
                {
                    bool isValid = await UserService.IsValid(user.token);
                    if (isValid)
                    {
                        UserAppService.SetLoggedIn(true);
                        DHttpClient.setToken(user.token);
                        Navigation.NavigateTo("/receipt");
                    }
                }
                catch (HttpRequestException)
                {
                    UserAppService.ClearUser();
                    DHttpClient.setToken(string.Empty);
                }
                catch (Exception)
                {
                    UserAppService.ClearUser();
                    DHttpClient.setToken(string.Empty);
                    Navigation.NavigateTo("/");
                }
            }
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
