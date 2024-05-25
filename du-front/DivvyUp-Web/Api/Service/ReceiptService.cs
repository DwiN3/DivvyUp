using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.Urls;
using Newtonsoft.Json;
using System.Text;
using DivvyUp_Web.Api.Models;

namespace DivvyUp_Web.Api.Service
{
    public class ReceiptService : IReceiptService
    {
        private Url _url { get; set; } = new();
        private HttpClient _httpClient { get; set; } = new();

        public async Task<HttpResponseMessage> AddReceipt(string token, ReceiptModel receipt)
        {
            System.Diagnostics.Debug.Print(receipt.date.ToString());
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var receiptData = new
            {
                receiptName = receipt.receiptName,
                date = receipt.date
            };

            var jsonData = JsonConvert.SerializeObject(receiptData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_url.AddReceipt, content);
            return response;
        }

        public async Task<HttpResponseMessage> SetSettled(string token, ReceiptModel receipt)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var settledData = new
            {
                settled = receipt.isSettled,
            };

            var jsonData = JsonConvert.SerializeObject(settledData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(_url.SetSettled + receipt.receiptId, content);
            return response;
        }

        public async Task<HttpResponseMessage> Remove(string token, ReceiptModel receipt)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync(_url.ReceiptRemove+ receipt.receiptId);
            return response;
        }

        public async Task<HttpResponseMessage> ShowAll(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(_url.ShowAll);
            return response;
        }
    }
}
