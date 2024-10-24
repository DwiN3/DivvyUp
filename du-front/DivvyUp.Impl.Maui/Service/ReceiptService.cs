using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Newtonsoft.Json;
using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.AppConstants;

namespace DivvyUp_Impl_Maui.Service
{
    public class ReceiptService : IReceiptService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private ApiRoute _url { get; } = new();
        private readonly ILogger<ReceiptService> _logger;

        public ReceiptService(DHttpClient dHttpClient, ILogger<ReceiptService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task Add(ReceiptDto receipt)
        {
            try
            {
                var url = _url.AddReceipt;
                var response = await _dHttpClient.PostAsync(url, receipt);
                await EnsureCorrectResponse(response, "Błąd w czasie dodawania rachunku");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania rachunku: {Message}", ex.Message);
                throw;
            }
        }

        public async Task Edit(ReceiptDto receipt)
        {
            try
            {
                var url = _url.EditReceipt.Replace(ApiRoute.arg_ID, receipt.id.ToString());
                var response = await _dHttpClient.PutAsync(url, receipt);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji rachunku");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji rachunku: {Message}", ex.Message);
                throw;
            }
        }

        public async Task Remove(int receiptId)
        {
            try
            {
                var url = _url.RemoveReceipt.Replace(ApiRoute.arg_ID, receiptId.ToString());
                var response = await _dHttpClient.DeleteAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji rachunku");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji rachunku: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetSettled(int receiptId, bool settled)
        {
            try
            {
                var url = _url.SetSettledReceipt
                    .Replace(ApiRoute.arg_ID, receiptId.ToString())
                    .Replace(ApiRoute.arg_Settled, settled.ToString());
                var response = await _dHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji rachunku");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji rachunku: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetTotalPrice(int receiptId, double totalPrice)
        {
            try
            { 
                var url = _url.SetTotalPriceReceipt
                    .Replace(ApiRoute.arg_ID, receiptId.ToString())
                    .Replace(ApiRoute.arg_TotalPrice, totalPrice.ToString());
                var response = await _dHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji rachunku");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
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
                var url = _url.GetReceipt.Replace(ApiRoute.arg_ID, receiptId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ReceiptDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie rachunku");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
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
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ReceiptDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie rachunków");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy rachunków do tabeli: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ReceiptDto>> GetReceiptsByDataRange(DateTime? from, DateTime? to)
        {
            try
            {
                if (!from.HasValue || !to.HasValue)
                    throw new ArgumentException("Obie daty muszą być podane");

                string fromFormatted = from.Value.ToString("dd-MM-yyyy");
                string toFormatted = to.Value.ToString("dd-MM-yyyy");

                var url = _url.GetReceiptsByDataRange
                    .Replace(ApiRoute.arg_From, fromFormatted)
                    .Replace(ApiRoute.arg_To, toFormatted);
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ReceiptDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobierania rachunków w zakresie dat");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania rachunków w zakresie dat: {Message}", ex.Message);
                throw;
            }
        }


    private async Task EnsureCorrectResponse(HttpResponseMessage response, string errorMessage)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogError("{ErrorMessage} Kod '{StatusCode}'. Response: '{Response}'", errorMessage, response.StatusCode, content);
                throw new DException(response.StatusCode, content);
            }
        }
    }
}
