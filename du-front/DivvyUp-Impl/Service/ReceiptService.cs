using DivvyUp.Shared.Model;
using Microsoft.AspNetCore.Components;
using DivvyUp.Shared.Dto;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;
using DivvyUp.Shared.Interface;
using DivvyUp_Impl.Api.DuHttpClient;
using DivvyUp_Impl.Api.Route;
using Newtonsoft.Json;

namespace DivvyUp_Impl.Service
{
    public class ReceiptService : IReceiptService
    {
        [Inject]
        private DuHttpClient _duHttpClient { get; set; }
        private readonly Route _url;
        private readonly ILogger<ReceiptService> _logger;


        public ReceiptService(DuHttpClient duHttpClient, Route url, ILogger<ReceiptService> logger)
        {
            _duHttpClient = duHttpClient;
            _url = url;
            _logger = logger;
        }

        public async Task AddReceipt(ReceiptDto receipt)
        {
            try
            {
                if (receipt == null)
                    throw new InvalidOperationException("Nie mozna dodać pustego rachunku");
                if (receipt.receiptName.Equals(string.Empty))
                    throw new InvalidOperationException("Nie mozna dodać rachunku bez nazwy");

                var url = _url.AddReceipt;
                var response = await _duHttpClient.PostAsync(url, receipt);
                await EnsureCorrectResponse(response, "Błąd w czasie dodawania rachunku");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania rachunku: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania rachunku: {Message}", ex.Message);
                throw;
            }
        }

        public async Task EditReceipt(ReceiptDto receipt)
        {
            try
            {
                if (receipt == null)
                    throw new InvalidOperationException("Nie mozna dodać pustego rachunku");
                if (receipt.receiptName.Equals(string.Empty))
                    throw new InvalidOperationException("Nie mozna dodać rachunku bez nazwy");

                var url = _url.EditReceipt.Replace(Route.ID, receipt.receiptId.ToString());
                var response = await _duHttpClient.PutAsync(url, receipt);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji rachunku");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji rachunku: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji rachunku: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetSettled(int receiptId, bool isSettled)
        {
            try
            {
                if (receiptId == null)
                    throw new InvalidOperationException("Nie mozna rozliczyć rachunku nie posiadającego id");

                var data = new
                {
                    settled = isSettled
                };

                var url = _url.SetSettled.Replace(Route.ID, receiptId.ToString());
                var response = await _duHttpClient.PutAsync(url, data);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji rachunku");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji rachunku: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji rachunku: {Message}", ex.Message);
                throw;
            }
        }

        public async Task RemoveReceipt(int receiptId)
        {
            try
            {
                if (receiptId == null)
                    throw new InvalidOperationException("Nie mozna usunąć rachunku które nie posiada id");

                var url = _url.ReceiptRemove.Replace(Route.ID, receiptId.ToString());
                var response = await _duHttpClient.DeleteAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji rachunku");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji rachunku: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji rachunku: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ReceiptDto>> ShowAllReceipts()
        {
            try
            {
                var url = _url.ShowAll;
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ReceiptDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie rachunków");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy rachunków do tabeli: {Message}", ex.Message);
                throw;
            }
        }

        private async Task EnsureCorrectResponse(HttpResponseMessage response, string errorMessage)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogError("{ErrorMessage} Kod '{StatusCode}'. Response: '{Response}'", errorMessage, response.StatusCode, content);

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new InvalidOperationException(content);
                }

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
