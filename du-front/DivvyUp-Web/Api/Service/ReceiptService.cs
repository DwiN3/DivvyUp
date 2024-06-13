using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.Urls;
using DivvyUp_Web.Api.Models;
using Microsoft.AspNetCore.Components;
using DivvyUp_Web.DivvyUpHttpClient;

namespace DivvyUp_Web.Api.Service
{
    public class ReceiptService : IReceiptService
    {
        [Inject]
        private DuHttpClient _duHttpClient { get; set; }
        private readonly Route _url;

        public ReceiptService(DuHttpClient duHttpClient, Route url)
        {
            _duHttpClient = duHttpClient;
            _url = url;
        }

        public async Task<HttpResponseMessage> AddReceipt(Receipt receipt)
        {
            var data = new
            {
                receiptName = receipt.receiptName,
                date = receipt.date
            };

            var response = await _duHttpClient.PostAsync(_url.AddReceipt, data);
            return response;
        }

        public async Task<HttpResponseMessage> SetSettled(int receiptId, bool isSettled)
        {
            var data = new
            {
                settled = isSettled
            };

            string url = _url.SetSettled.Replace(Route.ID, receiptId.ToString());
            var response = await _duHttpClient.PutAsync(url, data);
            return response;
        }

        public async Task<HttpResponseMessage> Remove(int receiptId)
        {
            string url = _url.ReceiptRemove.Replace(Route.ID, receiptId.ToString());
            var response = await _duHttpClient.DeleteAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage> ShowAll()
        {
            var response = await _duHttpClient.GetAsync(_url.ShowAll);
            return response;
        }
    }
}
