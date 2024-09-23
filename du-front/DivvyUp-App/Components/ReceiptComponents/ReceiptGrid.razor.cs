using DivvyUp_Web.Api.Dtos;
using DivvyUp_Web.Api.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace DivvyUp_App.Components.ReceiptComponents
{
    partial class ReceiptGrid : ComponentBase
    {
        [Inject]
        private IReceiptService ReceiptService { get; set; }

        public List<ReceiptDto> Receipts { get; set; }
        private RadzenDataGrid<ReceiptDto> receiptGrid { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            Receipts = await ReceiptService.ShowAll();
            StateHasChanged();
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
            var receipt = new ReceiptDto();
            Receipts.Add(receipt);
            await receiptGrid.InsertRow(receipt);
        }


        private async Task SaveRow(ReceiptDto r)
        {
            if (r.receiptId == 0)
                await ReceiptService.AddReceipt(r);
            else
                await ReceiptService.EditReceipt(r);

            await RefreshGrid();
        }

        private async Task EditRow(ReceiptDto order)
        {
            await receiptGrid.EditRow(order);
        }

        private void CancelEdit(ReceiptDto receipt)
        {
            receiptGrid.CancelEditRow(receipt);
        }

        private async void RemoveReceipt(int receiptId)
        {
            await ReceiptService.Remove(receiptId);
            await RefreshGrid();
        }
    }
}
