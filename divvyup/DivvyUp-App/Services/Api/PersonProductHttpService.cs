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
    public class PersonProductHttpService : IPersonProductService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private readonly ILogger<PersonProductHttpService> _logger;

        public PersonProductHttpService(DHttpClient dHttpClient, ILogger<PersonProductHttpService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task Add(AddEditPersonProductDto personProduct, int productId)
        {
            try
            {
                var url = ApiRoute.PERSON_PRODUCT_ROUTES.ADD
                    .Replace(ApiRoute.ARG_PRODUCT, productId.ToString()); ;
                var response = await _dHttpClient.PostAsync(url, personProduct);
                await EnsureCorrectResponse(response, "Błąd w czasie dodawania produktu osób");
            }
            catch (DException ex)
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

        public async Task Edit(AddEditPersonProductDto personProduct, int personProductId)
        {
            try
            {
                var url = ApiRoute.PERSON_PRODUCT_ROUTES.EDIT
                    .Replace(ApiRoute.ARG_PERSON_PRODUCT, personProductId.ToString());
                var response = await _dHttpClient.PutAsync(url, personProduct);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu");
            }
            catch (DException ex)
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

        public async Task Remove(int personProductId)
        {
            try
            {
                var url = ApiRoute.PERSON_PRODUCT_ROUTES.REMOVE
                    .Replace(ApiRoute.ARG_PERSON_PRODUCT, personProductId.ToString());
                var response = await _dHttpClient.DeleteAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie usuwania produktu osób");
            }
            catch (DException ex)
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

        public async Task SetPerson(int personProductId, int personId)
        {
            try
            {
                var url = ApiRoute.PERSON_PRODUCT_ROUTES.SET_PERSON
                    .Replace(ApiRoute.ARG_PERSON_PRODUCT, personProductId.ToString())
                    .Replace(ApiRoute.ARG_PERSON, personId.ToString());
                var response = await _dHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu osoby");
            }
            catch (DException ex)
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

        public async Task SetSettled(int personProductId, bool settled)
        {
            try
            {
                var url = ApiRoute.PERSON_PRODUCT_ROUTES.SET_SETTLED
                    .Replace(ApiRoute.ARG_PERSON_PRODUCT, personProductId.ToString())
                    .Replace(ApiRoute.ARG_SETTLED, settled.ToString());

                var response = await _dHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu osoby");
            }
            catch (DException ex)
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

        public async Task SetCompensation(int personProductId)
        {
            try
            {
                var url = ApiRoute.PERSON_PRODUCT_ROUTES.SET_COMPENSATION
                    .Replace(ApiRoute.ARG_PERSON_PRODUCT, personProductId.ToString());
                var response = await _dHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu");
            }
            catch (DException ex)
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
                var url = ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCT
                    .Replace(ApiRoute.ARG_PERSON_PRODUCT, personProductId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PersonProductDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktu osób");
                return result;
            }
            catch (DException ex)
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
                var url = ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCT_FROM_PRODUCT
                    .Replace(ApiRoute.ARG_PRODUCT, productId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<PersonProductDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktów osób");
                return result;
            }
            catch (DException ex)
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
                var url = ApiRoute.PERSON_PRODUCT_ROUTES.PERSON_PRODUCTS;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<PersonProductDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktów osób");
                return result;
            }
            catch (DException ex)
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
                _logger.LogError("{ErrorMessage} Kod '{Status}'. Response: '{Response}'", errorMessage, response.StatusCode, content);
                throw new DException(response.StatusCode, content);
            }
        }
    }
}
