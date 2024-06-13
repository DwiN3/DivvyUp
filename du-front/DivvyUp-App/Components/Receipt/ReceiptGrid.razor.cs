using BlazorBootstrap;
using DivvyUp_Web.Api.Response;
using DivvyUp_Web.Api.Interface;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using DivvyUp_Web.Api.Models;
using Newtonsoft.Json;

namespace DivvyUp_App.Components.Receipt
{
    partial class ReceiptGrid : ComponentBase
    {
        [Inject]
        private IReceiptService ReceiptService { get; set; }
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }

        public List<ShowReceiptResponse> Receipts { get; set; }


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
            else
            {
                System.Diagnostics.Debug.Print(response.StatusCode.ToString());
            }
        }

        private async Task<GridDataProviderResult<ShowReceiptResponse>> ReceiptsDataProvider(GridDataProviderRequest<ShowReceiptResponse> request)
        {
            if (Receipts == null)
            {
                return new GridDataProviderResult<ShowReceiptResponse>
                {
                    Data = Enumerable.Empty<ShowReceiptResponse>(),
                    TotalCount = 0
                };
            }

            return await Task.FromResult(request.ApplyTo(Receipts));
        }

        private void SetSettled(int receiptId, bool isChecked)
        {
            ReceiptService.SetSettled(receiptId, isChecked);
        }

        private async void RemoveReceipt(int receiptId)
        {
            var response = await ReceiptService.Remove(receiptId);
            if (response.IsSuccessStatusCode)
            {
                await LoadGrid();
            }
        }

        public async Task RefreshGrid()
        {
            await LoadGrid();
        }
    }
}
