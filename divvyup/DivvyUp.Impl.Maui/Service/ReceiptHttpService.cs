﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Newtonsoft.Json;
using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Model;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DivvyUp_Impl_Maui.Service
{
    public class ReceiptHttpService : IReceiptHttpService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private readonly ILogger<ReceiptHttpService> _logger;

        public ReceiptHttpService(DHttpClient dHttpClient, ILogger<ReceiptHttpService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task Add(ReceiptDto receipt)
        {
            try
            {
                var url = ApiRoute.RECEIPT_ROUTES.ADD;
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
                var url = ApiRoute.RECEIPT_ROUTES.EDIT
                    .Replace(ApiRoute.ARG_RECEIPT, receipt.id.ToString());
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

        public async Task<List<ReceiptDto>> GetReceiptsByDataRange(DateTime? from, DateTime? to)
        {
            try
            {
                if (!from.HasValue || !to.HasValue)
                    throw new ArgumentException("Obie daty muszą być podane");

                string fromFormatted = from.Value.ToString("dd-MM-yyyy");
                string toFormatted = to.Value.ToString("dd-MM-yyyy");

                var url = ApiRoute.RECEIPT_ROUTES.RECEIPTS_DATA_RANGE
                    .Replace(ApiRoute.ARG_FROM, fromFormatted)
                    .Replace(ApiRoute.ARG_TO, toFormatted);
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