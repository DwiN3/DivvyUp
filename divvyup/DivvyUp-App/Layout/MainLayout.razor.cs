using Blazored.LocalStorage;
using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.HttpClients;
using DivvyUp_Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

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
        private bool IsLogged = false;

        protected override async void OnInitialized()
        {
            Navigation.LocationChanged += OnLocationChanged;
            UserStateProvider.OnUserStateChanged += OnUserStateChanged;
            await SetUser();
            SetHeader(Navigation.Uri);
            StateHasChanged();
        }

        private async Task SetUser()
        {
            var token = await UserStateProvider.GetTokenAsync();
            await UserStateProvider.ClearTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    bool isValid = await UserService.ValidToken(token);
                    if (isValid)
                    {
                        await UserStateProvider.SetTokenAsync(token);
                    }
                }
                catch (HttpRequestException)
                {
                    await UserStateProvider.ClearTokenAsync();
                    StateHasChanged();
                }
                catch (Exception)
                {
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

        private async void OnUserStateChanged()
        {
            IsLogged = await UserStateProvider.IsLoggedInAsync();
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
            Navigation.NavigateTo("/");
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
            UserStateProvider.OnUserStateChanged -= OnUserStateChanged;
        }
    }
}
