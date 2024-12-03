using Blazored.LocalStorage;
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
        private HeaderService HeaderService { get; set; }
        [Inject]
        private ILocalStorageService LocalStorageService {get; set; }
        [Inject]
        private UserStateProvider UserStateProvider { get; set; }

        private bool SidebarExpanded { get; set; } = false;
        private string Header { get; set; } = string.Empty;
        private bool IsUserLoggedIn { get; set; }

        protected override async void OnInitialized()
        {
            Navigation.LocationChanged += OnLocationChanged;
            await SetUser();
            SetHeader(Navigation.Uri);
            StateHasChanged();
        }

        private async Task SetUser()
        {
            var token = await UserStateProvider.GetTokenAsync();
            await UserStateProvider.SetTokenAsync(string.Empty);
            IsUserLoggedIn = true;

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    bool isValid = await UserService.ValidToken(token);
                    if (isValid)
                    {
                        IsUserLoggedIn = true;
                        await UserStateProvider.SetTokenAsync(token);
                    }
                }
                catch (HttpRequestException)
                {
                    IsUserLoggedIn = true;
                    await UserStateProvider.ClearTokenAsync();
                    StateHasChanged();
                }
                catch (Exception)
                {
                    IsUserLoggedIn = true;
                    await UserStateProvider.ClearTokenAsync();
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
            await UserStateProvider.ClearTokenAsync();
            StateHasChanged();
            Navigation.NavigateTo("/", true);
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }
    }
}
