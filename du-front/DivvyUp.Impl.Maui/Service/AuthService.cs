using DivvyUp_Impl_Maui.Api.DuHttpClient;
using DivvyUp_Impl_Maui.Api.Route;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;

namespace DivvyUp_Impl_Maui.Service
{
    public class AuthService : IAuthService
    {
        [Inject]
        private DuHttpClient _duHttpClient { get; set; }
        private Route _url { get; } = new();
        private readonly ILogger<ReceiptService> _logger;

        public AuthService(DuHttpClient duHttpClient, ILogger<ReceiptService> logger)
        {
            _duHttpClient = duHttpClient;
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
                var response = await _duHttpClient.PostAsync(url, user);
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
                var response = await _duHttpClient.PostAsync(url, user);
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

        public async Task RemoveAccount()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsValid(string token)
        {
            try
            {
                if (token == null)
                    throw new InvalidOperationException("Brak tokenu");

                var url = $"{_url.IsValid}?token={token}";
                var response = await _duHttpClient.GetAsync(url);
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

        private async Task EnsureCorrectResponse(HttpResponseMessage response, string errorMessage)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogError("{ErrorMessage} Kod '{StatusCode}'. Response: '{Response}'", errorMessage, response.StatusCode, content);

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new InvalidOperationException(content);
                }

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
