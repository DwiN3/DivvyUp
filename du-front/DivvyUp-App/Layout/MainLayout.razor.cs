using Blazored.LocalStorage;
using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.DuHttp;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Layout
{
    partial class MainLayout
    {
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }
        [Inject]
        private IAuthService AuthService { get; set; }
        [Inject]
        private DuHttpClient DuHttpClient { get; set; }

        protected override async void OnInitialized()
        {
            var token = await LocalStorage.GetItemAsync<string>("authToken");
            var response = await AuthService.isValid(token);

            if (response.IsSuccessStatusCode)
            {
                await LocalStorage.SetItemAsync("isLogin", true);
                DuHttpClient.UpdateToken(token);
                Navigation.NavigateTo("/receipt");
            }
            
            else
            {
                DuHttpClient.UpdateToken(string.Empty);
                await LocalStorage.SetItemAsync("authToken", string.Empty);
                await LocalStorage.SetItemAsync("isLogin", false);
                Navigation.NavigateTo("/");
            }
        }

        private async Task Logout()
        {
            await LocalStorage.SetItemAsync("authToken", string.Empty);
            await LocalStorage.SetItemAsync("isLogin", false);
            Navigation.NavigateTo("/");
        }
    }
}
