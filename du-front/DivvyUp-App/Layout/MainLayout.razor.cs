using Blazored.LocalStorage;
using DivvyUp_Impl.Interface;
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
        private IAuthService AuthService { get; set; }
        [Inject]
        private DuHttpClient DuHttpClient { get; set; }

        [Inject]
        private UserAppService User { get; set; }

        protected override async void OnInitialized()
        {
            var user = User.GetUser();

            if (!string.IsNullOrEmpty(user.token))
            {
                var response = await AuthService.isValid(user.token);

                if (response.IsSuccessStatusCode)
                {
                    User.SetUser(user.username, user.token, true);
                    DuHttpClient.UpdateToken(user.token);
                    Navigation.NavigateTo("/receipt");
                }
                else
                {
                    User.ClearUser();
                    DuHttpClient.UpdateToken(string.Empty);
                    Navigation.NavigateTo("/");
                }
            }
            else
            {
                Navigation.NavigateTo("/");
            }
            StateHasChanged();
        }

        private async Task Logout()
        {
            User.ClearUser();
            StateHasChanged();
            Navigation.NavigateTo("/");
        }
    }
}
