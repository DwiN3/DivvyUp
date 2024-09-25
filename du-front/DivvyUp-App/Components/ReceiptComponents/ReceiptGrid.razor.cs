using DivvyUp_Web.Api.Dtos;
using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen.Blazor;

namespace DivvyUp_App.Components.ReceiptComponents
{
    partial class ReceiptGrid : ComponentBase
    {
        [Inject]
        private IReceiptService ReceiptService { get; set; }

        public List<ReceiptDto> Receipts { get; set; }
        private RadzenDataGrid<ReceiptDto> receiptGrid { get; set; }
        IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        protected override async Task OnInitializedAsync()
        {
            await LoadGrid();
        }

        private async Task LoadGrid()
        {
            Receipts = await ReceiptService.ShowAllReceipts();
            StateHasChanged();
        }

        private async Task InsertRow()
        {
            var receipt = new ReceiptDto();
            Receipts.Add(receipt);
            await receiptGrid.InsertRow(receipt);
        }

        private async Task EditRow(ReceiptDto receipt)
        {
            await receiptGrid.EditRow(receipt);
        }

        private void CancelEdit(ReceiptDto receipt)
        {
            receiptGrid.CancelEditRow(receipt);
        }

        private async Task SaveRow(ReceiptDto r)
        {
            try
            {
                if (r.receiptId == 0)
                    await ReceiptService.AddReceipt(r);
                else
                    await ReceiptService.EditReceipt(r);
            } 
            catch (InvalidOperationException ex)
            {
            } 
            catch (Exception ex)
            {
            } 
            finally
            {
                await LoadGrid();
            }
        }

        private async void RemoveReceipt(int receiptId)
        {
            try
            {
                await ReceiptService.RemoveReceipt(receiptId);
            }
            catch (InvalidOperationException ex)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                await LoadGrid();
            }
        }

        private async Task SetSettled(int receiptId, bool isChecked)
        {
            try
            {
                await ReceiptService.SetSettled(receiptId, isChecked);
            }
            catch (InvalidOperationException ex)
            {
            }
            catch (Exception ex)
            {
            }
        }
    }
}
