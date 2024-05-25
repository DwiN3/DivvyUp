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
        private List<ShowReceiptResponse> Receipts { get; set; }
        private string Token { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Token = await LocalStorage.GetItemAsync<string>("authToken");
            if (Token != null)
            {
                await LoadGrid();
            }
        }

        private async Task LoadGrid()
        {
            var response = await ReceiptService.ShowAll(Token);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Receipts = JsonConvert.DeserializeObject<List<ShowReceiptResponse>>(responseBody);
                StateHasChanged();
            }
        }

        private async Task Logout()
        {
            await LocalStorage.SetItemAsync("authToken", "");
            Navigation.NavigateTo("/");
        }
    }
}
