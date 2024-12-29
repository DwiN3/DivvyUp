using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.HttpClients;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Interfaces;

namespace DivvyUp_App.Services.Api
{
    public class ReceiptHttpService : IReceiptService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private readonly ILogger<ReceiptHttpService> _logger;

        public ReceiptHttpService(DHttpClient dHttpClient, ILogger<ReceiptHttpService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task Add(AddEditReceiptDto receipt)
        {
            try
            {
                var url = ApiRoute.RECEIPT_ROUTES.ADD;
                var response = await _dHttpClient.PutAsync(url, receipt);
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

        public async Task Edit(AddEditReceiptDto receipt, int receiptId)
        {
            try
            {
                var url = ApiRoute.RECEIPT_ROUTES.EDIT
                    .Replace(ApiRoute.ARG_RECEIPT, receiptId.ToString());
                var response = await _dHttpClient.PatchAsync(url, receipt);
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
                var url = ApiRoute.RECEIPT_ROUTES.REMOVE
                    .Replace(ApiRoute.ARG_RECEIPT, receiptId.ToString());
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
                var url = ApiRoute.RECEIPT_ROUTES.SET_SETTLED
                    .Replace(ApiRoute.ARG_RECEIPT, receiptId.ToString())
                    .Replace(ApiRoute.ARG_SETTLED, settled.ToString());
                var response = await _dHttpClient.PatchAsync(url);
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

        public async Task<ReceiptDto> GetReceipt(int receiptId)
        {
            try
            {
                var url = ApiRoute.RECEIPT_ROUTES.RECEIPT
                    .Replace(ApiRoute.ARG_RECEIPT, receiptId.ToString());
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
                var url = ApiRoute.RECEIPT_ROUTES.RECEIPTS;
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

        public async Task<List<ReceiptDto>> GetReceiptsByDataRange(DateOnly from, DateOnly to)
        {
            try
            {
                var url = $"{ApiRoute.RECEIPT_ROUTES.RECEIPTS_DATA_RANGE}?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}";
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
                _logger.LogError("{ErrorMessage} Kod '{Status}'. Response: '{Response}'", errorMessage, response.StatusCode, content);
                throw new DException(response.StatusCode, content);
            }
        }
    }
}