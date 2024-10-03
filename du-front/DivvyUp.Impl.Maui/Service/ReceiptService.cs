using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using DivvyUp_Impl_Maui.Api.DuHttpClient;
using DivvyUp_Impl_Maui.Api.Route;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Newtonsoft.Json;
using DivvyUp_Impl_Maui.Api.HttpResponseException;

namespace DivvyUp_Impl_Maui.Service
{
    public class ReceiptService : IReceiptService
    {
        [Inject]
        private DuHttpClient _duHttpClient { get; set; }
        private Route _url { get; } = new();
        private readonly ILogger<ReceiptService> _logger;

        public ReceiptService(DuHttpClient duHttpClient, ILogger<ReceiptService> logger)
        {
            _duHttpClient = duHttpClient;
            _logger = logger;
        }

        public async Task AddReceipt(ReceiptDto receipt)
        {
            try
            {
                if (receipt == null)
                    throw new InvalidOperationException("Nie mozna dodać pustego rachunku");
                if (receipt.name.Equals(string.Empty))
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
                    throw new InvalidOperationException("Nie mozna edytować pustego rachunku");
                if (receipt.name.Equals(string.Empty))
                    throw new InvalidOperationException("Nie mozna edytować rachunku bez nazwy");

                var url = _url.EditReceipt.Replace(Route.ID, receipt.id.ToString());
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

        public async Task RemoveReceipt(int receiptId)
        {
            try
            {
                if (receiptId == null)
                    throw new InvalidOperationException("Nie mozna usunąć rachunku które nie posiada id");

                var url = _url.RemoveReceipt.Replace(Route.ID, receiptId.ToString());
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

        public async Task SetSettledReceipt(int receiptId, bool isSettled)
        {
            try
            {
                if (receiptId == null)
                    throw new InvalidOperationException("Nie mozna rozliczyć rachunku nie posiadającego id");

                var data = new
                {
                    isSettled = isSettled
                };

                var url = _url.SetSettledReceipt.Replace(Route.ID, receiptId.ToString());
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

        public async Task SetTotalPriceReceipt(int receiptId, double totalPrice)
        {
            try
            {
                if (receiptId == null)
                    throw new InvalidOperationException("Nie mozna ustawić ceny rachunku nie posiadającego id");

                var data = new
                {
                    totalPrice = totalPrice
                };

                var url = _url.SetTotalPriceReceipt.Replace(Route.ID, receiptId.ToString());
                var response = await _duHttpClient.PutAsync(url, data);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji rachunku");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji ceny rachunku: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji ceny rachunku: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ReceiptDto> GetReceipt(int receiptId)
        {
            try
            {
                var url = _url.GetReceipt.Replace(Route.ID, receiptId.ToString());
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ReceiptDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie rachunku");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania rachunku: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ReceiptDto>> GetReceipts()
        {
            try
            {
                var url = _url.GetReceipts;
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
                throw new HttpResponseException(response.StatusCode, errorMessage);
            }
        }
    }
}
