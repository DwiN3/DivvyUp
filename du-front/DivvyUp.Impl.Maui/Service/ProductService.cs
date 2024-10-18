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
    public class ProductService : IProductService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private ApiRoute _url { get; } = new();
        private readonly ILogger<ProductService> _logger;

        public ProductService(DHttpClient dHttpClient, ILogger<ProductService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }

        public async Task<ProductDto> Add(ProductDto product)
        {
            try
            {
                var url = _url.AddProduct.Replace(ApiRoute.arg_ID, product.receiptId.ToString());
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

        public async Task<ProductDto> Edit(ProductDto product)
        {
            try
            {
                var url = _url.EditProduct.Replace(ApiRoute.arg_ID, product.id.ToString());
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

        public async Task Remove(int productId)
        {
            try
            {
                var url = _url.RemoveProduct.Replace(ApiRoute.arg_ID, productId.ToString());
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
                var url = _url.SetSettledProduct
                    .Replace(ApiRoute.arg_ID, productId.ToString())
                    .Replace(ApiRoute.arg_Settled, settled.ToString());
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

        public async Task SetCompensationPrice(int productId, double compensationPrice)
        {
            try
            {
                var url = _url.SetCompensationPriceProduct
                    .Replace(ApiRoute.arg_ID, productId.ToString())
                    .Replace(ApiRoute.arg_CompensationPrice, compensationPrice.ToString());
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
                var url = _url.GetProduct.Replace(ApiRoute.arg_ID, productId.ToString());
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

        public async Task<List<ProductDto>> GetProducts(int receiptId)
        {
            try
            {
                var url = _url.GetProducts.Replace(ApiRoute.arg_ID, receiptId.ToString());
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
                _logger.LogError("{ErrorMessage} Kod '{StatusCode}'. Response: '{Response}'", errorMessage, response.StatusCode, content);
                throw new DException(response.StatusCode, content);
            }
        }
    }
}
