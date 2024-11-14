using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using DivvyUp_Shared.RequestDto;
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
        private DNotificationService DNotificationService { get; set; }
        [Inject]
        private DDialogService DDialogService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private List<ReceiptDto> Receipts { get; set; }
        private RadzenDataGrid<ReceiptDto> Grid { get; set; }
        private IEnumerable<int> PageSizeOptions = new int[] { 5, 10, 25, 50, 100 };
        private DateOnly DateFrom { get; set; }
        private DateOnly DateTo { get; set; }
        private bool ShowAllReceipts = false;
        private bool IsGridEdit { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await SetCurrentMonth();
        }

        private async Task LoadGrid()
        {
            try
            {
                if (ShowAllReceipts)
                    Receipts = await ReceiptService.GetReceipts();
                else
                    Receipts = await ReceiptService.GetReceiptsByDataRange(DateFrom, DateTo);
            }
            catch (DException ex)
            {
                DNotificationService.ShowNotification(ex.Message, NotificationSeverity.Error);
            }
            catch (Exception)
            {
            }
            await Grid.Reload();
            StateHasChanged();
        }


        private async Task InsertRow()
        {
            IsGridEdit = true;
            var receipt = new ReceiptDto();
            await Grid.InsertRow(receipt);
        }

        private async Task EditRow(ReceiptDto receipt)
        {
            IsGridEdit = true;
            await Grid.EditRow(receipt);
        }

        private void CancelEdit(ReceiptDto receipt)
        {
            IsGridEdit = false;
            Grid.CancelEditRow(receipt);
        }

        private async Task SaveRow(ReceiptDto receipt)
        {
            IsGridEdit = false;
            try
            {
                AddEditReceiptRequest request = new()
                {
                    Name = receipt.name,
                    Date = receipt.date
                };

                if (receipt.id == 0)
                {
                    await ReceiptService.Add(request);
                }
                else
                {
                    await ReceiptService.Edit(request, receipt.id);
                }
            }
            catch (DException ex)
            {
                DNotificationService.ShowNotification(ex.Message, NotificationSeverity.Error);
            }
            catch (Exception)
            {
            }
            finally
            {
                await LoadGrid();
                StateHasChanged();
            }
        }


        private async Task RemoveRow(ReceiptDto receipt)
        {
            IsGridEdit = false;
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

        private async Task SetCurrentMonth()
        {
            DateFrom = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1);
            int dayInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTo = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, dayInMonth);
            await LoadGrid();
        }
    }
}
