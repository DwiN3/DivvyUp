using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace DivvyUp_App.Components.Receipt
{
    partial class ReceiptGrid : ComponentBase
    {
        [Inject]
        private IReceiptService ReceiptService { get; set; }
        [Inject]
        private DAlertService AlertService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

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

        private async Task RemoveRow(int receiptId)
        {
            try
            {
                await ReceiptService.RemoveReceipt(receiptId);
                AlertService.ShowAlert("Usunięto rachunek", AlertStyle.Success);
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

        private async Task OpenProductsList(int receiptId)
        {
            if (receiptId > 0)
            {
                Navigation.NavigateTo($"/receipt/{receiptId}/products");
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
