using DivvyUp.Shared.Dto;
using DivvyUp_Web.Api.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen.Blazor;

namespace DivvyUp_App.Components.ReceiptComponents
{
    partial class ReceiptGrid : ComponentBase
    {
        [Inject]
        private IReceiptService ReceiptService { get; set; }

        private List<ReceiptDto> Receipts { get; set; }
        private RadzenDataGrid<ReceiptDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };

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
            await Grid.InsertRow(receipt);
        }

        private async Task EditRow(ReceiptDto receipt)
        {
            await Grid.EditRow(receipt);
        }

        private void CancelEdit(ReceiptDto receipt)
        {
            Grid.CancelEditRow(receipt);
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

        private async void RemoveRow(int receiptId)
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

        private async Task ChangeSettled(int receiptId, bool isChecked)
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
