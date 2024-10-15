using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Impl_Maui.Api.HttpResponseException;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Exceptions;
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

        public async Task<ProductDto> AddProduct(ProductDto product)
        {
            try
            {
                var url = _url.AddProduct.Replace(ApiRoute.ID, product.receiptId.ToString());
                var response = await _dHttpClient.PostAsync(url, product);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktu");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductDto>(jsonResponse);
                return result;
            }
            catch (DuException ex)
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

        public async Task<ProductDto> EditProduct(ProductDto product)
        {
            try
            {
                var url = _url.EditProduct.Replace(ApiRoute.ID, product.id.ToString());
                var response = await _dHttpClient.PutAsync(url, product);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductDto>(jsonResponse);
                return result;
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

        public async Task RemoveProduct(int productId)
        {
            try
            {
                var url = _url.RemoveProduct.Replace(ApiRoute.ID, productId.ToString());
                var response = await _dHttpClient.DeleteAsync(url);
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

        public async Task SetSettledProduct(int productId, bool settled)
        {
            try
            {
                var url = _url.SetSettledProduct
                    .Replace(ApiRoute.ID, productId.ToString())
                    .Replace(ApiRoute.Settled, settled.ToString());
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
                _logger.LogError(ex, "Błąd w czasie edycji produktu: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetCompensationPriceProduct(int productId, double compensationPrice)
        {
            try
            {
                var url = _url.SetCompensationPriceProduct
                    .Replace(ApiRoute.ID, productId.ToString())
                    .Replace(ApiRoute.CompensationPrice, compensationPrice.ToString());
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
                _logger.LogError(ex, "Błąd w czasie edycji produktu: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ProductDto> GetProduct(int productId)
        {
            try
            {
                var url = _url.GetProduct.Replace(ApiRoute.ID, productId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktu");
                return result;
            }
            catch (DuException ex)
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
                var url = _url.GetProducts.Replace(ApiRoute.ID, receiptId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ProductDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktów");
                return result;
            }
            catch (DuException ex)
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
                throw new DuException(response.StatusCode, content);
            }
        }
    }
}
