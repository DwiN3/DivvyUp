using BlazorBootstrap;
using DivvyUp_Web.Api.Response;
using DivvyUp_Web.Api.Interface;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using DivvyUp_Web.Api.Models;
using Newtonsoft.Json;
using Radzen.Blazor;

namespace DivvyUp_App.Components.ReceiptComponents
{
    partial class ReceiptGrid : ComponentBase
    {
        [Inject]
        private IReceiptService ReceiptService { get; set; }

        public List<ReceiptModel> Receipts { get; set; }
        private RadzenDataGrid<ReceiptModel> receiptGrid { get; set; }

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
                Receipts = JsonConvert.DeserializeObject<List<ReceiptModel>>(responseBody);
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

        private async Task InsertRow()
        {
            var receipt = new ReceiptModel();
            Receipts.Add(receipt);
            await receiptGrid.InsertRow(receipt);
        }


        private async Task SaveRow(ReceiptModel r)
        {
            if (r.receiptId == 0)
                await ReceiptService.AddReceipt(r);
            else
                await ReceiptService.EditReceipt(r);

            await RefreshGrid();
        }

        private async Task EditRow(ReceiptModel order)
        {
            await receiptGrid.EditRow(order);
        }

        private void CancelEdit(ReceiptModel receipt)
        {
            receiptGrid.CancelEditRow(receipt);
        }

        private async void RemoveReceipt(int receiptId)
        {
            var response = await ReceiptService.Remove(receiptId);
            if (response.IsSuccessStatusCode)
                await RefreshGrid();
        }
    }
}
