using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DivvyUp_Impl_Maui.Service
{
    public class ChartHttpService : IChartService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private readonly ILogger<ChartHttpService> _logger;

        public ChartHttpService(DHttpClient dHttpClient, ILogger<ChartHttpService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task<List<ChartDto>> GetAmounts(bool isTotalAmounts)
        {
            try
            {
                var url = isTotalAmounts ? ApiRoute.CHART_ROUTES.TOTAL_AMOUNTS : ApiRoute.CHART_ROUTES.UNPAID_AMOUNTS;
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

        public async Task<List<ChartDto>> GetPercentageExpanses()
        {
            try
            {
                var url = ApiRoute.CHART_ROUTES.PERCENTAGE_EXPENSES;
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
                var url = ApiRoute.CHART_ROUTES.MONTHLY_TOTAL_EXPANSES
                    .Replace(ApiRoute.ARG_YEAR, year.ToString());
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
                var url = ApiRoute.CHART_ROUTES.MONTHLY_USER_EXPANSES
                    .Replace(ApiRoute.ARG_YEAR, year.ToString());
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
                var url = ApiRoute.CHART_ROUTES.WEEKLY_TOTAL_EXPENSES;
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
                var url = ApiRoute.CHART_ROUTES.WEEKLY_USER_EXPENSES;
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
                var url = ApiRoute.CHART_ROUTES.MONTHLY_TOP_PRODUCTS;
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
                _logger.LogError("{ErrorMessage} Kod '{Status}'. Response: '{Response}'", errorMessage, response.StatusCode, content);
                throw new DException(response.StatusCode, content);
            }
        }
    }
}
