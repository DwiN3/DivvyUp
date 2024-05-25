using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var token = await LocalStorage.GetItemAsync<string>("authToken");
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
