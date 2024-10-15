using DivvyUp_Impl_Maui.Api.HttpResponseException;
using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DivvyUp_Impl_Maui.Service
{
    public class PersonProductService : IPersonProductService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private ApiRoute _url { get; } = new();
        private readonly ILogger<PersonProductService> _logger;

        public PersonProductService(DHttpClient dHttpClient, ILogger<PersonProductService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task AddPersonProduct(PersonProductDto personProduct, int productId)
        {
            try
            {
                var url = _url.AddPersonProduct.Replace(ApiRoute.arg_ID, productId.ToString()); ;
                var response = await _dHttpClient.PostAsync(url, personProduct);
                await EnsureCorrectResponse(response, "Błąd w czasie dodawania produktu osób");
            }
            catch (DuException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania produktu osób: {Message}", ex.Message);
                throw;
            }
        }

        public async Task EditPersonProduct(PersonProductDto personProduct)
        {
            try
            {
                var url = _url.EditPersonProduct.Replace(ApiRoute.arg_ID, personProduct.id.ToString());
                var response = await _dHttpClient.PutAsync(url, personProduct);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu");
            }
            catch (DuException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu: {Message}", ex.Message);
                throw;
            }
        }

        public async Task RemovePersonProduct(int personProductId)
        {
            try
            {
                var url = _url.RemovePersonProduct.Replace(ApiRoute.arg_ID, personProductId.ToString());
                var response = await _dHttpClient.DeleteAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie usuwania produktu osób");
            }
            catch (DuException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie usuwania produktu osób: {Message}", ex.Message);
                throw;
            }
        }

        public async Task ChangePersonPersonProduct(int personProductId, int personId)
        {
            try
            {
                var url = _url.ChangePersonPersonProduct
                    .Replace(ApiRoute.arg_ID, personProductId.ToString())
                    .Replace(ApiRoute.arg_PersonId, personId.ToString());
                var response = await _dHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu osoby");
            }
            catch (DuException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu osoby: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetSettledPersonProduct(int personProductId, bool settled)
        {
            try
            {
                var url = _url.SetSettledPersonProduct
                    .Replace(ApiRoute.arg_ID, personProductId.ToString())
                    .Replace(ApiRoute.arg_Settled, settled.ToString());

                var response = await _dHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu osoby");
            }
            catch (DuException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu osoby: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetCompensationPersonProduct(int personProductId)
        {
            try
            {
                var url = _url.SetCompensationPersonProduct.Replace(ApiRoute.arg_ID, personProductId.ToString());
                var response = await _dHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu");
            }
            catch (DuException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu osoby: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<PersonProductDto> GetPersonProduct(int personProductId)
        {
            try
            {
                var url = _url.GetPersonProduct.Replace(ApiRoute.arg_ID, personProductId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PersonProductDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktu osób");
                return result;
            }
            catch (DuException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy produktu osób do tabeli: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<PersonProductDto>> GetPersonProductsFromProduct(int productId)
        {
            try
            {
                var url = _url.GetPersonProductsForProduct.Replace(ApiRoute.arg_ID, productId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<PersonProductDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktów osób");
                return result;
            }
            catch (DuException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy produktów osób do tabeli: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<PersonProductDto>> GetPersonProducts()
        {
            try
            {
                var url = _url.GetPersonProducts;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<PersonProductDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktów osób");
                return result;
            }
            catch (DuException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy produktów osób do tabeli: {Message}", ex.Message);
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
