using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.HttpClients;
using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DivvyUp_App.Services.Api
{
    public class ProductHttpService : IProductService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private readonly ILogger<ProductHttpService> _logger;

        public ProductHttpService(DHttpClient dHttpClient, ILogger<ProductHttpService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task<ProductDto> Add(AddEditProductDto product, int receiptId)
        {
            try
            {
                var url = ApiRoute.PRODUCT_ROUTES.ADD
                    .Replace(ApiRoute.ARG_RECEIPT, receiptId.ToString());
                var response = await _dHttpClient.PostAsync(url, product);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktu");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductDto>(jsonResponse);
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania produktu: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ProductDto> Edit(AddEditProductDto product, int productId)
        {
            try
            {
                var url = ApiRoute.PRODUCT_ROUTES.EDIT
                    .Replace(ApiRoute.ARG_PRODUCT, productId.ToString());
                var response = await _dHttpClient.PutAsync(url, product);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductDto>(jsonResponse);
                return result;
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

        public async Task AddWithPerson(AddEditProductDto request, int receiptId, int personId)
        {
            try
            {
                var url = ApiRoute.PRODUCT_ROUTES.ADD_WIDTH_PERSON
                    .Replace(ApiRoute.ARG_RECEIPT, receiptId.ToString())
                    .Replace(ApiRoute.ARG_PERSON, personId.ToString());
                var response = await _dHttpClient.PostAsync(url, request);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktu");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania produktu: {Message}", ex.Message);
                throw;
            }
        }

        public async Task EditWithPerson(AddEditProductDto request, int productId, int personId)
        {
            try
            {
                var url = ApiRoute.PRODUCT_ROUTES.EDIT_WIDTH_PERSON
                    .Replace(ApiRoute.ARG_PRODUCT, productId.ToString())
                    .Replace(ApiRoute.ARG_PERSON, personId.ToString());
                var response = await _dHttpClient.PutAsync(url, request);
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

        public async Task Remove(int productId)
        {
            try
            {
                var url = ApiRoute.PRODUCT_ROUTES.REMOVE
                    .Replace(ApiRoute.ARG_PRODUCT, productId.ToString());
                var response = await _dHttpClient.DeleteAsync(url);
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

        public async Task SetSettled(int productId, bool settled)
        {
            try
            {
                var url = ApiRoute.PRODUCT_ROUTES.SET_SETTLED
                    .Replace(ApiRoute.ARG_PRODUCT, productId.ToString())
                    .Replace(ApiRoute.ARG_SETTLED, settled.ToString());
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
                _logger.LogError(ex, "Błąd w czasie edycji produktu: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ProductDto> GetProduct(int productId)
        {
            try
            {
                var url = ApiRoute.PRODUCT_ROUTES.PRODUCT
                    .Replace(ApiRoute.ARG_PRODUCT, productId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktu");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy produktu do tabeli: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            try
            {
                var url = ApiRoute.PRODUCT_ROUTES.PRODUCTS;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ProductDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktów");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy produktów do tabeli: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ProductDto>> GetProductsFromReceipt(int receiptId)
        {
            try
            {
                var url = ApiRoute.PRODUCT_ROUTES.PRODUCTS_FROM_RECEIPT
                    .Replace(ApiRoute.ARG_RECEIPT, receiptId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ProductDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktów");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy produktów do tabeli: {Message}", ex.Message);
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
