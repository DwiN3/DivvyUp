using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace DivvyUp_App.Components.Receipt
{
    partial class ReceiptGrid : ComponentBase
    {
        [Inject]
        private IReceiptService ReceiptService { get; set; }
        [Inject]
        private DNotificationService DNotificationService { get; set; }
        [Inject]
        private DDialogService DDialogService { get; set; }
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
            Receipts = await ReceiptService.GetReceipts();
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

        private async Task SaveRow(ReceiptDto receipt)
        {
            try
            {
                if (receipt.id == 0)
                    await ReceiptService.Add(receipt);
                else
                    await ReceiptService.Edit(receipt);
            }
            catch (DException ex)
            {
            }
            catch (Exception)
            {
            }
            finally
            {
                await LoadGrid();
            }
        }

        private async Task RemoveRow(ReceiptDto receipt)
        {
            try
            {
                var result = await DDialogService.OpenYesNoDialog("Usuwanie rachunku", $"Czy potwierdzasz usunięcie rachunku: {receipt.name}?");
                if (result)
                    await ReceiptService.Remove(receipt.id);
            }
            catch (DException ex)
            {
            }
            catch (Exception)
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
            catch (DException ex)
            {
            }
            catch (Exception)
            {
            }
        }
    }
}
