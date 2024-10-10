using DivvyUp_Impl_Maui.Api.HttpResponseException;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Shared.AppConstants;

namespace DivvyUp_Impl_Maui.Service
{
    public class AuthService : IAuthService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private ApiRoute _url { get; } = new();
        private readonly ILogger<ReceiptService> _logger;

        public AuthService(DHttpClient dHttpClient, ILogger<ReceiptService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task<string> Login(UserDto user)
        {
            try
            {
                if (user == null)
                    throw new InvalidOperationException("Dane są puste");
                if (user.username == null || user.username.Equals(string.Empty))
                    throw new InvalidOperationException("Nazwa jest pusta");
                if (user.password == null || user.password.Equals(string.Empty))
                    throw new InvalidOperationException("Hasło jest puste");

                var url = _url.Login;
                var response = await _dHttpClient.PostAsync(url, user);
                var result = await response.Content.ReadAsStringAsync();
                await EnsureCorrectResponse(response, "Błąd w czasie logowania");
                return result;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie logowania: {Message}", ex.Message);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd w czasie logowania: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie logowania: {Message}", ex.Message);
                throw;
            }
        }
        public async Task Register(UserDto user)
        {
            try
            {
                if (user == null)
                    throw new InvalidOperationException("Dane są puste");
                if (user.username == null || user.username.Equals(string.Empty))
                    throw new InvalidOperationException("Nazwa jest pusta");
                if (user.password == null || user.password.Equals(string.Empty))
                    throw new InvalidOperationException("Hasło jest puste");
                if (user.email == null || user.email.Equals(string.Empty))
                    throw new InvalidOperationException("Email");

                var url = _url.Register;
                var response = await _dHttpClient.PostAsync(url, user);
                await EnsureCorrectResponse(response, "Błąd w czasie rejestracji");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie rejestracji: {Message}", ex.Message);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd w czasie rejestracji: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie rejestracji: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<string> EditUser(UserDto user)
        {
            try
            {
                if (user == null)
                    throw new InvalidOperationException("Nie mozna edytować pustego użytkownika");
                if (user.username.Equals(string.Empty))
                    throw new InvalidOperationException("Nie mozna edytować użytkownika bez nazwy");
                if (user.email.Equals(string.Empty))
                    throw new InvalidOperationException("Nie mozna edytować użytkownika bez emaila");

                var url = _url.EditUser;
                var response = await _dHttpClient.PutAsync(url, user);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji użytkownika"); 
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji użytkownika: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji użytkownika: {Message}", ex.Message);
                throw;
            }
        }

        public async Task ChangePassword(string password, string newPassword)
        {
            try
            {
                var data = new
                {
                    password = password,
                    newPassword = newPassword
                };

                var url = _url.ChangePasswordUser;
                var response = await _dHttpClient.PutAsync(url, data);
                await EnsureCorrectResponse(response, "Błąd w czasie zmieniania hasła");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie zmieniania hasła\": {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie zmieniania hasła\" {Message}", ex.Message);
                throw;
            }
        }

        public async Task RemoveUser()
        {
            try
            {
                var url = _url.RemoveUser;
                var response = await _dHttpClient.DeleteAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie usuwania użytkownika");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie usuwania użytkownika: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie usuwania użytkownika: {Message}", ex.Message);
                throw;
            }
        }


        public async Task<bool> IsValid(string token)
        {
            try
            {
                if (token == null)
                    throw new InvalidOperationException("Brak tokenu");

                var url = $"{_url.IsValid}?token={token}";
                var response = await _dHttpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();
                bool result = bool.Parse(responseContent);
                await EnsureCorrectResponse(response, "Błąd w czasie walidacji");
                return result;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie rejestracji: {Message}", ex.Message);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd w czasie walidacji: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie walidacji: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<UserDto> GetUser(string token)
        {
            try
            {
                var url = $"{_url.GetUser}?token={token}";
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<UserDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie użytkownika");
                return result;
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
                _logger.LogError("{ErrorMessage} Kod '{StatusCode}'. Response: '{Response}'", errorMessage, response.StatusCode, content);
                throw new HttpResponseException(response.StatusCode, errorMessage);
            }
        }
    }
}
