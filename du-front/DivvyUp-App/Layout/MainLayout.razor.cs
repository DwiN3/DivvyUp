using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Layout
{
    partial class MainLayout
    {
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }
        private string Token { get; set; }

        protected override async void OnInitialized()
        {
            Token = await LocalStorage.GetItemAsync<string>("authToken");
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
