using DivvyUp_Web.Api.Interface;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_App.Components.ReceiptComponents
{
    partial class ReceiptAdd
    {
        [Inject]
        private IReceiptService ReceiptService { get; set; }
        [Parameter]
        public EventCallback RefreshGrid { get; set; }

        private string NameReceipt { get; set; }
        private DateTime DateReceipt { get; set; } = DateTime.Now;
        private bool IsDelivery { get; set; }
        private double CostOfDelivery { get; set; }


        protected override async Task OnInitializedAsync()
        {
            
        }

        private async Task AddReceipt()
        {
            DivvyUp_Web.Api.Models.ReceiptModel receipt = new();
            receipt.receiptName = NameReceipt;
            receipt.date = (DateTime)DateReceipt;

            if (IsDelivery)
            {
                CostOfDelivery = 0;
            }

            var response = await ReceiptService.AddReceipt(receipt);
            if (response.IsSuccessStatusCode)
            {
                await RefreshGrid.InvokeAsync(null);
            }
        }
    }
}
