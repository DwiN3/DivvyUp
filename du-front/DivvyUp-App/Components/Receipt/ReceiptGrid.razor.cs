using BlazorBootstrap;
using DivvyUp_Web.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DivvyUp_Web.Api.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Blazored.LocalStorage;
using Newtonsoft.Json;

namespace DivvyUp_App.Components.Receipt
{
    partial class ReceiptGrid : ComponentBase
    {
        public List<ShowReceiptResponse> Receipts { get; set; }
        [Inject]
        private IReceiptService ReceiptService { get; set; }
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }
        private string Token { get; set; }

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
            System.Diagnostics.Debug.Print("Id: "+receiptId+" Wartość: "+isChecked);
            ReceiptService.SetSettled(Token, receiptId, isChecked);
        }

        private async void RemoveReceipt(int receiptId)
        {
            var response = await ReceiptService.Remove(Token, receiptId);
            if (response.IsSuccessStatusCode)
            {
                await LoadGrid();
            }
        }
    }
}
