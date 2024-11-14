using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.HttpClients;
using DivvyUp_Shared.Interfaces;
using Microsoft.AspNetCore.Components;

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
        private UserAppService UserAppService { get; set; }
        [Inject]
        private HeaderService HeaderService { get; set; } 

        private bool SidebarExpanded { get; set; } = false;
        private string Header { get; set; } = string.Empty;

        protected override async void OnInitialized()
        {
            Navigation.LocationChanged += OnLocationChanged;
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
                    bool isValid = await UserService.ValidToken(user.token);
                    if (isValid)
                    {
                        UserAppService.SetLoggedIn(true);
                        DHttpClient.setToken(user.token);
                    }
                }
                catch (HttpRequestException)
                {
                    UserAppService.ClearUser();
                    DHttpClient.setToken(string.Empty);
                    StateHasChanged();
                }
                catch (Exception)
                {
                    UserAppService.ClearUser();
                    DHttpClient.setToken(string.Empty);
                    StateHasChanged();
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

        private async Task Logout()
        {
            UserAppService.ClearUser();
            StateHasChanged();
            Navigation.NavigateTo("/", true);
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }
    }
}
