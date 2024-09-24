using Blazored.LocalStorage;
using DivvyUp_Impl.Model;
using DivvyUp_Impl.Service;
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

        [Inject]
        private UserService UserService { get; set; }

        protected override async void OnInitialized()
        {
            var user = UserService.GetUser();

            if (!string.IsNullOrEmpty(user.token))
            {
                var response = await AuthService.isValid(user.token);

                if (response.IsSuccessStatusCode)
                {
                    UserService.SetUser(user.username, user.token, true);
                    DuHttpClient.UpdateToken(user.token);
                    Navigation.NavigateTo("/receipt");
                }
                else
                {
                    UserService.ClearUser();
                    DuHttpClient.UpdateToken(string.Empty);
                    Navigation.NavigateTo("/");
                }
            }
            else
            {
                Navigation.NavigateTo("/");
            }
        }

        private async Task Logout()
        {
            UserService.ClearUser();
            Navigation.NavigateTo("/");
        }
    }
}
