using BlazorBootstrap;
using DivvyUp_Web.Api.Response;
using DivvyUp_Web.Api.Interface;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using DivvyUp_Web.Api.Models;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;

namespace DivvyUp_App.Components.Receipt
{
    partial class ReceiptGrid : ComponentBase
    {
        [Inject]
        private IReceiptService ReceiptService { get; set; }

        public List<ShowReceiptResponse> Receipts { get; set; }
        private RadzenDataGrid<ShowReceiptResponse> receiptGrid { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            var response = await ReceiptService.ShowAll();
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Receipts = JsonConvert.DeserializeObject<List<ShowReceiptResponse>>(responseBody);
                StateHasChanged();
            }
        }

        private async Task SetSettled(int receiptId, bool isChecked)
        {
            await ReceiptService.SetSettled(receiptId, isChecked);
        }

        private async void RemoveReceipt(int receiptId)
        {
            var response = await ReceiptService.Remove(receiptId);
            if (response.IsSuccessStatusCode)
            {
                await LoadGrid();
            }
        }

        public async Task RefreshGrid()
        {
            await LoadGrid();
        }
    }
}
