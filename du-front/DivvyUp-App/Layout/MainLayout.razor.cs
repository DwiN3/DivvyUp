using Blazored.LocalStorage;
using DivvyUp.Shared.Interface;
using DivvyUp_Impl.Api.DuHttpClient;
using DivvyUp_Impl.Service;
using Microsoft.AspNetCore.Components;

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
        private UserAppService User { get; set; }

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
            SetHeader(Navigation.Uri);

            var user = User.GetUser();

            if (!string.IsNullOrEmpty(user.token))
            {
                try
                {
                    await AuthService.isValid(user.token);
                    User.SetUser(user.username, user.token, true);
                    DuHttpClient.UpdateToken(user.token);
                    Navigation.NavigateTo("/receipt");
                }
                catch (HttpRequestException ex)
                {
                    User.ClearUser();
                    DuHttpClient.UpdateToken(string.Empty);
                    Navigation.NavigateTo("/");
                }
                catch (Exception ex)
                {
                    User.ClearUser();
                    DuHttpClient.UpdateToken(string.Empty);
                    Navigation.NavigateTo("/");
                    SetHeader("/");
                }
            }
            StateHasChanged();
        }

        private async Task Logout()
        {
            User.ClearUser();
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

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }
    }
}
