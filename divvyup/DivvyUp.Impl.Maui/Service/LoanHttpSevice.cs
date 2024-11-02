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
    public class LoanHttpSevice : ILoanHttpService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private ApiRoute _url { get; } = new();
        private readonly ILogger<LoanHttpSevice> _logger;

        public LoanHttpSevice(DHttpClient dHttpClient, ILogger<LoanHttpSevice> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task Add(LoanDto loan)
        {
            try
            {
                var url = _url.AddLoan;
                var response = await _dHttpClient.PostAsync(url, loan);
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

        public async Task Edit(LoanDto loan)
        {
            try
            {
                var url = _url.EditLoan.Replace(ApiRoute.arg_ID, loan.id.ToString());
                var response = await _dHttpClient.PutAsync(url, loan);
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
                var url = _url.RemoveLoan.Replace(ApiRoute.arg_ID, loanId.ToString());
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
                var url = _url.SetPersonLoan
                    .Replace(ApiRoute.arg_ID, loadId.ToString())
                    .Replace(ApiRoute.arg_PersonId, personId.ToString());
                var response = await _dHttpClient.PutAsync(url);
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
                var url = _url.SetLentLoan
                    .Replace(ApiRoute.arg_ID, loanId.ToString())
                    .Replace(ApiRoute.arg_Lent, lent.ToString());
                var response = await _dHttpClient.PutAsync(url);
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
                var url = _url.SetSettledLoan
                    .Replace(ApiRoute.arg_ID, loanId.ToString())
                    .Replace(ApiRoute.arg_Settled, isSettled.ToString());
                var response = await _dHttpClient.PutAsync(url);
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
                var url = _url.GetLoan.Replace(ApiRoute.arg_ID, loanId.ToString());
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

        public async Task<List<LoanDto>> GetLoansPerson(int personId)
        {
            try
            {
                var url = _url.GetLoanPerson.Replace(ApiRoute.arg_PersonId, personId.ToString());
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
                var url = _url.GetLoans;
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

        public async Task<List<LoanDto>> GetLoansByDataRange(DateTime? from, DateTime? to)
        {
            try
            {
                if (!from.HasValue || !to.HasValue)
                    throw new ArgumentException("Obie daty muszą być podane");

                string fromFormatted = from.Value.ToString("dd-MM-yyyy");
                string toFormatted = to.Value.ToString("dd-MM-yyyy");

                var url = _url.GetLoansByDataRange
                    .Replace(ApiRoute.arg_From, fromFormatted)
                    .Replace(ApiRoute.arg_To, toFormatted);
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
                _logger.LogError("{ErrorMessage} Kod '{StatusCode}'. Response: '{Response}'", errorMessage, response.StatusCode, content);
                throw new DException(response.StatusCode, content);
            }
        }
    }
}
