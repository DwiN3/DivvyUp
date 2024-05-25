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
        private string Token { get; set; }

        public List<ShowReceiptResponse> Receipts { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Token = await LocalStorage.GetItemAsync<string>("authToken");
            if (Token != null)
            {
                await LoadGrid();
            }
        }

        private async Task LoadGrid()
        {
            var response = await ReceiptService.ShowAll(Token);
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
            ReceiptModel receipt = new();
            receipt.receiptId = receiptId;
            receipt.isSettled = isChecked;
            ReceiptService.SetSettled(Token, receipt);
        }

        private async void RemoveReceipt(int receiptId)
        {
            ReceiptModel receipt = new();
            receipt.receiptId = receiptId;
            var response = await ReceiptService.Remove(Token, receipt);
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
