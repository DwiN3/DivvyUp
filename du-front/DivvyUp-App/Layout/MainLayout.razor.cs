using BlazorBootstrap;
using DivvyUp_App.Components.DAlert;
using DivvyUp_Impl_Maui.Api.DuHttpClient;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DivvyUp_App.Layout
{
    partial class MainLayout
    {
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private IAuthService AuthService { get; set; }
        [Inject]
        private DuHttpClient DuHttpClient { get; set; }
        [Inject]
        private IAlertService AlertService { get; set; }
        [Inject]
        private IUserAppService UserAppService { get; set; }

        private DAlert Alert { get; set; }
        private bool SidebarExpanded { get; set; } = false;

        Dictionary<string, string> MenuItems = new Dictionary<string, string>
        {
            { "/", "Strona Główna" },
            { "/receipt", "Rachunki" },
            { "/login", "Logowanie" },
            { "/register", "Rejestracja" },
            { "/logout", "Wyloguj się" }
        };
        
        private string Header { get; set; } = string.Empty;

        protected override async void OnInitialized()
        {
            Navigation.LocationChanged += OnLocationChanged;
            AlertService.OnAlert += ShowAlert;
            AlertService.OnCloseAlert += HideAlert;
            SetHeader(Navigation.Uri);

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
                catch (HttpRequestException ex)
                {
                    UserAppService.ClearUser();
                    DuHttpClient.UpdateToken(string.Empty);
                    Navigation.NavigateTo("/");
                }
                catch (Exception ex)
                {
                    UserAppService.ClearUser();
                    DuHttpClient.UpdateToken(string.Empty);
                    Navigation.NavigateTo("/");
                }
            }
            StateHasChanged();
        }

        private async Task Logout()
        {
            UserAppService.ClearUser();
            StateHasChanged();
            Navigation.NavigateTo("/");
        }

        private void OnLocationChanged(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            SetHeader(e.Location);
            StateHasChanged();
        }

        private void SetHeader(string url)
        {
            var relativePath = new Uri(url).AbsolutePath;

            if (MenuItems.ContainsKey(relativePath))
                Header = MenuItems[relativePath];
            else
                Header = string.Empty;
        }

        private async void ShowAlert(string message, AlertStyle style)
        {
            await Alert.OpenAlert(style, message);
        }

        private async void HideAlert()
        {
            await Alert.CloseAlert();
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
            AlertService.OnAlert -= ShowAlert;
            AlertService.OnCloseAlert -= HideAlert;
        }
    }
}
