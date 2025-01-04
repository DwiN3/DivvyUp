using Blazored.LocalStorage;
using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Models;
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
        private ILocalStorageService LocalStorageService { get; set; }
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
        private bool IsLoading => Receipts == null;

        private const string DateFromKey = "ReceiptGrid_DateFrom";
        private const string DateToKey = "ReceiptGrid_DateTo";
        private const string ShowAllReceiptsKey = "ReceiptGrid_ShowAllReceipts";

        protected override async Task OnInitializedAsync()
        {
            await LoadSettingsFromLocalStorage();
            await LoadGrid();
        }

        private async Task LoadSettingsFromLocalStorage()
        {
            var dateFrom = await LocalStorageService.GetItemAsync<string>(DateFromKey);
            var dateTo = await LocalStorageService.GetItemAsync<string>(DateToKey);
            var showAllReceipts = await LocalStorageService.GetItemAsync<bool?>(ShowAllReceiptsKey);

            if (dateFrom != null && dateTo != null)
            {
                DateFrom = DateOnly.Parse(dateFrom);
                DateTo = DateOnly.Parse(dateTo);
            }
            else
            {
                await SetCurrentMonth();
            }

            ShowAllReceipts = showAllReceipts ?? false;
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
            var receipt = new ReceiptDto();
            await Grid.InsertRow(receipt);
        }

        private async Task EditRow(ReceiptDto receipt)
        {
            await Grid.EditRow(receipt);
        }

        private async Task CancelEdit(ReceiptDto receipt)
        {
            Grid.CancelEditRow(receipt);
            await LoadGrid();
        }

        private async Task SaveRow(ReceiptDto receipt)
        {
            try
            {
                AddEditReceiptDto request = new(receipt.Name, receipt.Date, receipt.DiscountPercentage);

                if (receipt.Id == 0)
                {
                    await ReceiptService.Add(request);
                }
                else
                {
                    await ReceiptService.Edit(request, receipt.Id);
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
            try
            {
                var result = await DDialogService.OpenYesNoDialog("Usuwanie rachunku", $"Czy potwierdzasz usunięcie rachunku: {receipt.Name}?");
                if (result)
                    await ReceiptService.Remove(receipt.Id);
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
            await SaveSettingsToLocalStorage();
            await LoadGrid();
        }

        private async Task OpenDetails(bool edit, ReceiptDto receipt)
        {
            await DDialogService.OpenReceiptDetailsDialog(edit, receipt);
        }

        private async Task SaveSettingsToLocalStorage()
        {
            await LocalStorageService.SetItemAsync(DateFromKey, DateFrom.ToString());
            await LocalStorageService.SetItemAsync(DateToKey, DateTo.ToString());
            await LocalStorageService.SetItemAsync(ShowAllReceiptsKey, ShowAllReceipts);
        }
    }
}
