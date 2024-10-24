using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
namespace DivvyUp_Impl_Maui.Service
{
    public class ChartService : IChartService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private ApiRoute _url { get; } = new();
        private readonly ILogger<ChartService> _logger;

        public ChartService(DHttpClient dHttpClient, ILogger<ChartService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task<List<ChartDto>> GetTotalAmounts()
        {
            try
            {
                var url = _url.GetTotalAmountsChart;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ChartDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie wykresów wszystkich kosztów");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobieranie wykresów wszystkich kosztów: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ChartDto>> GetUnpaindAmounts()
        {
            try
            {
                var url = _url.GetUnpaidAmountsChart;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ChartDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie wykresów nie opłaconych kosztów");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobieranie wykresów nie opłaconych kosztów: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ChartDto>> GetPercantageExpenses()
        {
            try
            {
                var url = _url.GetPercentageExpensesChart;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ChartDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie procent opłaconych wydatków");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\"Błąd w czasie pobieranie procent opłaconych wydatków: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ChartDto>> GetMonthlyTotalExpenses(int year)
        {
            try
            {
                var url = _url.GetMonthlyTotalExpensesChart.Replace(ApiRoute.arg_Year, year.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ChartDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobierania miesięcznych wydatków");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\"Błąd w czasie pobierania miesięcznych wydatków: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ChartDto>> GetMonthlyUserExpenses(int year)
        {
            try
            {
                var url = _url.GetMonthlyUserExpenses.Replace(ApiRoute.arg_Year, year.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ChartDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobierania miesięcznych wydatków właściciela konta");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\"Błąd w czasie pobierania miesięcznych wydatków właściciela konta: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ChartDto>> GetWeeklyTotalExpenses()
        {
            try
            {
                var url = _url.GetWeeklyTotalExpenses;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ChartDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobierania tygoniowych wydatków osób");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\"Błąd w czasie pobierania tygoniowych wydatków osób: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ChartDto>> GetWeeklyUserExpenses()
        {
            try
            {
                var url = _url.GetWeeklyUserExpenses;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ChartDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobierania tygoniowych wydatków właściciela konta");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\"Błąd w czasie pobierania tygoniowych wydatków właściciela konta: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ChartDto>> GetMonthlyTopProducts()
        {
            try
            {
                var url = _url.GetMonthlyTopProducts;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ChartDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobierania największych wydatków na miesiąc");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\"Błąd w czasie pobierania największych wydatków na miesiąc: {Message}", ex.Message);
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
