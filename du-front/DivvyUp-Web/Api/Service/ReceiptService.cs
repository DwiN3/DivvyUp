using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.Urls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
