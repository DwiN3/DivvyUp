using System.Diagnostics;
using Blazored.LocalStorage;
using DivvyUp_Web.DivvyUpHttpClient;
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
        private DuHttpClient DuHttpClient { get; set; }
        private string Token { get; set; }

        protected override async void OnInitialized()
        {
            Token = await LocalStorage.GetItemAsync<string>("authToken");
            DuHttpClient.UpdateToken(Token);
            await StartUp();
        }

        private async Task StartUp()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                Navigation.NavigateTo("/receipt");
            }
            else
            {
                Navigation.NavigateTo("/");
            }
        }
    }
}
