using Blazored.LocalStorage;
using DivvyUp_Web.Api.Interface;
using Microsoft.AspNetCore.Components;
using DivvyUp_Web.Api.Models;

namespace DivvyUp_App.Components.Receipt
{
    partial class ReceiptAdd
    {
        [Inject]
        private IReceiptService ReceiptService { get; set; }
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }
        private string Token { get; set; }
        [Parameter]
        public EventCallback RefreshGrid { get; set; }

        private string NameReceipt { get; set; }
        private DateTime? DateReceipt { get; set; } = DateTime.Now;
        private bool IsDelivery { get; set; }
        private double CostOfDelivery { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Token = await LocalStorage.GetItemAsync<string>("authToken");
        }
        private async Task AddReceipt()
        {
            ReceiptModel receipt = new();
            receipt.receiptName = NameReceipt;
            receipt.date = (DateTime)DateReceipt;

            var response = await ReceiptService.AddReceipt(Token, receipt);
            if (response.IsSuccessStatusCode)
            {
                await RefreshGrid.InvokeAsync(null);
            }
        }
    }
}
