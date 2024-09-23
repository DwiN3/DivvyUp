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

        public async Task RefreshGrid()
        {
            await LoadGrid();
        }

        async Task InsertRow()
        {
            var receipt = new ShowReceiptResponse();
            Receipts.Add(receipt);
            await receiptGrid.InsertRow(receipt);
        }


        async Task SaveRow(ShowReceiptResponse receipt)
        {
            await receiptGrid.UpdateRow(receipt);
        }

        private async Task AddReceipt(ShowReceiptResponse r)
        {
            if (r.receiptId == 0)
            {
                DivvyUp_Web.Api.Models.Receipt receipt = new();
                receipt.receiptName = r.receiptName;
                receipt.date = r.date;
                await ReceiptService.AddReceipt(receipt);
            }
            else
            {
                DivvyUp_Web.Api.Models.Receipt receipt = new();
                receipt.receiptName = r.receiptName;
                receipt.date = r.date;
                receipt.receiptId = r.receiptId;
                await ReceiptService.EditReceipt(receipt);
            }

            await RefreshGrid();
        }

        async Task EditRow(ShowReceiptResponse order)
        {
            await receiptGrid.EditRow(order);
        }

        void CancelEdit(ShowReceiptResponse receipt)
        {
            receiptGrid.CancelEditRow(receipt);
        }

        private async void RemoveReceipt(int receiptId)
        {
            var response = await ReceiptService.Remove(receiptId);
            if (response.IsSuccessStatusCode)
            {
                await LoadGrid();
            }
        }
    }
}
