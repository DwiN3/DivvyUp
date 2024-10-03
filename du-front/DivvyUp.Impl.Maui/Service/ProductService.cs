using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DivvyUp_Impl_Maui.Api.DuHttpClient;
using DivvyUp_Impl_Maui.Api.HttpResponseException;
using DivvyUp_Impl_Maui.Api.Route;
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
        private DuHttpClient _duHttpClient { get; set; }
        private Route _url { get; } = new();
        private readonly ILogger<ProductService> _logger;

        public ProductService(DuHttpClient duHttpClient, ILogger<ProductService> logger)
        {
            _duHttpClient = duHttpClient;
            _logger = logger;
        }

        public async Task AddProduct(ProductDto product)
        {
            try
            {
                if (product == null)
                    throw new InvalidOperationException("Nie mozna dodać pustego produktu");
                if (product.name.Equals(string.Empty))
                    throw new InvalidOperationException("Nie mozna dodać productu bez nazwy");

                var url = _url.AddProduct.Replace(Route.ID, product.receiptId.ToString());
                var response = await _duHttpClient.PostAsync(url, product);
                await EnsureCorrectResponse(response, "Błąd w czasie dodawania produktu");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania produktu: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania produktu: {Message}", ex.Message);
                throw;
            }
        }

        public async Task EditProduct(ProductDto product)
        {
            try
            {
                if (product == null)
                    throw new InvalidOperationException("Nie mozna edytować pustego produktu");
                if (product.name.Equals(string.Empty))
                    throw new InvalidOperationException("Nie mozna edytować productu bez nazwy");

                var url = _url.EditProduct.Replace(Route.ID, product.id.ToString());
                var response = await _duHttpClient.PutAsync(url, product);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu: {Message}", ex.Message);
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
                if (productId == null)
                    throw new InvalidOperationException("Nie mozna usunąć produktu które nie posiada id");

                var url = _url.RemoveProduct.Replace(Route.ID, productId.ToString());
                var response = await _duHttpClient.DeleteAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetSettledProduct(int productId, bool isSettled)
        {
            try
            {
                if (productId == null)
                    throw new InvalidOperationException("Nie mozna rozliczyć produktu nie posiadającego id");

                var data = new
                {
                    isSettled = isSettled
                };

                var url = _url.SetSettledProduct.Replace(Route.ID, productId.ToString());
                var response = await _duHttpClient.PutAsync(url, data);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu: {Message}", ex.Message);
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
                if (productId == null)
                    throw new InvalidOperationException("Nie mozna ustawić ceny produktu nie posiadającego id");

                var data = new
                {
                    compensationPrice = compensationPrice
                };

                var url = _url.SetCompensationPriceProduct.Replace(Route.ID, productId.ToString());
                var response = await _duHttpClient.PutAsync(url, data);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu: {Message}", ex.Message);
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
                var url = _url.GetProduct.Replace(Route.ID, productId.ToString());
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktu");
                return result;
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
                var url = _url.GetProducts.Replace(Route.ID, receiptId.ToString());
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ProductDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktów");
                return result;
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
                throw new HttpResponseException(response.StatusCode, errorMessage);
            }
        }
    }
}
