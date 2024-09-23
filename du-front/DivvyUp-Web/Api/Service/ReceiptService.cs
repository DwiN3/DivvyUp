using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.Urls;
using DivvyUp_Web.Api.Models;
using Microsoft.AspNetCore.Components;
using DivvyUp_Web.Api.Dtos;
using Newtonsoft.Json;
using AutoMapper;
using DivvyUp_Web.DuHttp;

namespace DivvyUp_Web.Api.Service
{
    public class ReceiptService : IReceiptService
    {
        [Inject]
        private DuHttpClient _duHttpClient { get; set; }
        private readonly Route _url;
        private readonly IMapper _mapper;


        public ReceiptService(DuHttpClient duHttpClient, Route url, IMapper mapper)
        {
            _duHttpClient = duHttpClient;
            _url = url;
            _mapper = mapper;
        }

        public async Task AddReceipt(ReceiptDto receipt)
        {
            var data = new
            {
                receiptName = receipt.receiptName,
                date = receipt.date
            };

            var url = _url.AddReceipt;
            var response = await _duHttpClient.PostAsync(url, data);
        }

        public async Task EditReceipt(ReceiptDto receipt)
        {
            var url = _url.EditReceipt.Replace(Route.ID, receipt.receiptId.ToString());
            var response = await _duHttpClient.PutAsync(url, receipt);
        }

        public async Task SetSettled(int receiptId, bool isSettled)
        {
            var data = new
            {
                settled = isSettled
            };

            var url = _url.SetSettled.Replace(Route.ID, receiptId.ToString());
            var response = await _duHttpClient.PutAsync(url, data);
        }

        public async Task Remove(int receiptId)
        {
            var url = _url.ReceiptRemove.Replace(Route.ID, receiptId.ToString());
            var response = await _duHttpClient.DeleteAsync(url);
        }

        public async Task<List<ReceiptDto>> ShowAll()
        {
            var url = _url.ShowAll;
            var response = await _duHttpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var receiptModels = JsonConvert.DeserializeObject<List<ReceiptModel>>(jsonResponse);
                var result = _mapper.Map<List<ReceiptDto>>(receiptModels);

                return result;
            }
            return new List<ReceiptDto>();
        }
    }
}
