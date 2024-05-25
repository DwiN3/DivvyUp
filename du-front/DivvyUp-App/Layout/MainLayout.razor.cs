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


        protected override async Task OnInitializedAsync()
        {
            //await StartUp();
        }

        private async Task StartUp()
        {
            //var token = await LocalStorage.GetItemAsync<string>("authToken");
            var token =
                "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJLYW1pbCIsImlhdCI6MTcxNjY0OTc1MywiZXhwIjoxNzE2NjUyNjMzfQ.caFtTqtBd4SyN123H-a20FSNcZ4RpzzD-k6ghXVYdXY";
            await LocalStorage.SetItemAsync("authToken", token);
            if (!string.IsNullOrEmpty(token))
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
