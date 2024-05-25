using BlazorBootstrap;
using DivvyUp_Web.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DivvyUp_App.Components.Receipt
{
    partial class ReceiptGrid : ComponentBase
    {
        [Parameter]
        public List<ShowReceiptResponse> Receipts { get; set; }

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
        }
    }
}
