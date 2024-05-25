using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.Urls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using DivvyUp_Web.Api.Response;
using Blazored.LocalStorage;

namespace DivvyUp_Web.Api.Service
{
    public class ReceiptService : IReceiptService
    {
        private Url _url { get; set; } = new();
        private HttpClient _http { get; set; } = new();

        public async Task<HttpResponseMessage> ShowAll(string token)
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.GetAsync(_url.ShowAll);
            return response;
        }

        public async Task<HttpResponseMessage> Remove(string token, int id)
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _http.DeleteAsync(_url.ReceiptRemove+id);
            return response;
        }

        public async Task<HttpResponseMessage> SetSettled(string token, int id, bool settled)
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var settledData = new
            {
                settled = settled,
            };

            var jsonData = JsonConvert.SerializeObject(settledData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _http.PutAsync(_url.SetSettled+id, content);
            return response;
        }
    }
}
