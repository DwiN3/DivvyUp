using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.HttpClients;
using DivvyUp_Shared.RequestDto;
using static DivvyUp_Shared.AppConstants.ApiRoute;

namespace DivvyUp_App.Service.Api
{
    public class UserHttpService : IUserService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private readonly ILogger<ReceiptHttpService> _logger;

        public UserHttpService(DHttpClient dHttpClient, ILogger<ReceiptHttpService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task<string> Login(LoginUserRequest request)
        {
            try
            {
                var url = USER_ROUTES.LOGIN;
                var response = await _dHttpClient.PostAsync(url, request);
                var result = await response.Content.ReadAsStringAsync();
                await EnsureCorrectResponse(response, "Błąd w czasie logowania");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd w czasie logowania: {Message}", ex.Message);
                throw new TimeoutException("Nie można połączyć się z serwerem");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie logowania: {Message}", ex.Message);
                throw;
            }
        }

        public async Task Register(RegisterUserRequest request)
        {
            try
            {
                var url = ApiRoute.USER_ROUTES.REGISTER;
                var response = await _dHttpClient.PostAsync(url, request);
                await EnsureCorrectResponse(response, "Błąd w czasie rejestracji");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd w czasie logowania: {Message}", ex.Message);
                throw new TimeoutException("Nie można połączyć się z serwerem");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie rejestracji: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<string> Edit(EditUserRequest request)
        {
            try
            {
                var url = USER_ROUTES.EDIT;
                var response = await _dHttpClient.PutAsync(url, request);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji użytkownika"); 
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd w czasie logowania: {Message}", ex.Message);
                throw new TimeoutException("Nie można połączyć się z serwerem");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji użytkownika: {Message}", ex.Message);
                throw;
            }
        }

        public async Task ChangePassword(ChangePasswordUserRequest request)
        {
            try
            {
                var url = USER_ROUTES.CHANGE_PASSWORD;
                var response = await _dHttpClient.PutAsync(url, request);
                await EnsureCorrectResponse(response, "Błąd w czasie zmieniania hasła");
            }
            catch(DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd w czasie logowania: {Message}", ex.Message);
                throw new TimeoutException("Nie można połączyć się z serwerem");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie zmieniania hasła\" {Message}", ex.Message);
                throw;
            }
        }

        public async Task Remove()
        {
            try
            {
                var url = USER_ROUTES.REMOVE;
                var response = await _dHttpClient.DeleteAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie usuwania użytkownika");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd w czasie logowania: {Message}", ex.Message);
                throw new TimeoutException("Nie można połączyć się z serwerem");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie usuwania użytkownika: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<bool> ValidToken(string token)
        {
            try
            {
                var url = USER_ROUTES.VALIDATE_TOKEN
                    .Replace(ARG_TOKEN, token);
                var response = await _dHttpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();
                bool result = bool.Parse(responseContent);
                await EnsureCorrectResponse(response, "Błąd w czasie walidacji");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd w czasie logowania: {Message}", ex.Message);
                throw new TimeoutException("Nie można połączyć się z serwerem");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie walidacji: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<UserDto> GetUser()
        {
            try
            {
                var url = USER_ROUTES.ME;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<UserDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie użytkownika");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd w czasie logowania: {Message}", ex.Message);
                throw new TimeoutException("Nie można połączyć się z serwerem");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania użytkownika: {Message}", ex.Message);
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
