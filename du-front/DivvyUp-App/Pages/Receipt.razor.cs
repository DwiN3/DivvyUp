using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.Response;
using Newtonsoft.Json;
using BlazorBootstrap;

namespace DivvyUp_App.Pages
{
    partial class Receipt
    {
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }
        [Inject]
        private IReceiptService ReceiptService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        private string Token { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Token = await LocalStorage.GetItemAsync<string>("authToken");
        }

        private async Task Logout()
        {
            await LocalStorage.SetItemAsync("authToken", "");
            Navigation.NavigateTo("/");
        }
    }
}
