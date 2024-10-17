using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interface;
using DivvyUp_Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public async Task<List<ChartData>> GetTotalAmounts()
        {
            try
            {
                var url = _url.GetTotalAmountsChart;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ChartData>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie wykresów wszystkich kosztów");
                return result;
            }
            catch (DuException ex)
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

        public async Task<List<ChartData>> GetUnpaindAmounts()
        {
            try
            {
                var url = _url.GetUnpaidAmountsChart;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ChartData>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie wykresów nie opłaconych kosztów");
                return result;
            }
            catch (DuException ex)
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

        public async Task<List<ChartData>> GetPercantageExpenses()
        {
            try
            {
                var url = _url.GetPercentageExpensesChart;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ChartData>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie procent opłaconych wydatków");
                return result;
            }
            catch (DuException ex)
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

        private async Task EnsureCorrectResponse(HttpResponseMessage response, string errorMessage)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogError("{ErrorMessage} Kod '{StatusCode}'. Response: '{Response}'", errorMessage, response.StatusCode, content);
                throw new DuException(response.StatusCode, content);
            }
        }
    }
}
