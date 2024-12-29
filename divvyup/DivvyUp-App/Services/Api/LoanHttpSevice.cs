using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.HttpClients;
using DivvyUp_Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DivvyUp_App.Services.Api
{
    public class LoanHttpSevice : ILoanService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private readonly ILogger<LoanHttpSevice> _logger;

        public LoanHttpSevice(DHttpClient dHttpClient, ILogger<LoanHttpSevice> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task Add(AddEditLoanDto loan)
        {
            try
            {
                var url = ApiRoute.LOAN_ROUTES.ADD;
                var response = await _dHttpClient.PutAsync(url, loan);
                await EnsureCorrectResponse(response, "Błąd w czasie dodawaniu pożyczki");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania pożyczki: {Message}", ex.Message);
                throw;
            }
        }

        public async Task Edit(AddEditLoanDto loan, int loanId)
        {
            try
            {
                var url = ApiRoute.LOAN_ROUTES.EDIT
                    .Replace(ApiRoute.ARG_LOAN, loanId.ToString());
                var response = await _dHttpClient.PatchAsync(url, loan);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji pożyczki");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji pożyczki: {Message}", ex.Message);
                throw;
            }
        }

        public async Task Remove(int loanId)
        {
            try
            {
                var url = ApiRoute.LOAN_ROUTES.REMOVE
                    .Replace(ApiRoute.ARG_LOAN, loanId.ToString());
                var response = await _dHttpClient.DeleteAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie usuwania pożyczki");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie usuwania pożyczki: {Message}", ex.Message);
                throw;
            }
        }

        public  async Task SetPerson(int loadId, int personId)
        {
            try
            {
                var url = ApiRoute.LOAN_ROUTES.SET_PERSON
                    .Replace(ApiRoute.ARG_LOAN, loadId.ToString())
                    .Replace(ApiRoute.ARG_PERSON, personId.ToString());
                var response = await _dHttpClient.PatchAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji pożyczki");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji pożyczki: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetLent(int loanId, bool lent)
        {
            try
            {
                var url = ApiRoute.LOAN_ROUTES.SET_LENT
                    .Replace(ApiRoute.ARG_LOAN, loanId.ToString())
                    .Replace(ApiRoute.ARG_LENT, lent.ToString());
                var response = await _dHttpClient.PatchAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetSettled(int loanId, bool isSettled)
        {
            try
            {
                var url = ApiRoute.LOAN_ROUTES.SET_SETTLED
                    .Replace(ApiRoute.ARG_LOAN, loanId.ToString())
                    .Replace(ApiRoute.ARG_SETTLED, isSettled.ToString());
                var response = await _dHttpClient.PatchAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<LoanDto> GetLoan(int loanId)
        {
            try
            {
                var url = ApiRoute.LOAN_ROUTES.LOAN
                    .Replace(ApiRoute.ARG_LOAN, loanId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LoanDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie pożyczki");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobieranie pożyczki: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<LoanDto>> GetPersonLoans(int personId)
        {
            try
            {
                var url = ApiRoute.LOAN_ROUTES.LOANS_PERSON
                    .Replace(ApiRoute.ARG_PERSON, personId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<LoanDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie pożyczki");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobieranie pożyczki: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<LoanDto>> GetLoans()
        {
            try
            {
                var url = ApiRoute.LOAN_ROUTES.LOANS;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<LoanDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie pożyczki");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobieranie pożyczki: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<LoanDto>> GetLoansByDataRange(DateOnly from, DateOnly to)
        {
            try
            {
                var url = $"{ApiRoute.LOAN_ROUTES.LOANS_DATA_RANGE}?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}";
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<LoanDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobierania pożyczek w zakresie dat");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania pożyczek w zakresie dat: {Message}", ex.Message);
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
