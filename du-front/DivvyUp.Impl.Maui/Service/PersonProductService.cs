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
    public class PersonProductService : IPersonProductService
    {
        [Inject]
        private DuHttpClient _duHttpClient { get; set; }
        private Route _url { get; } = new();
        private readonly ILogger<PersonProductService> _logger;

        public PersonProductService(DuHttpClient duHttpClient, ILogger<PersonProductService> logger)
        {
            _duHttpClient = duHttpClient;
            _logger = logger;
        }

        public async Task AddPersonProduct(PersonProductDto personProduct, int productId)
        {
            try
            {
                if (personProduct == null)
                    throw new InvalidOperationException("Nie mozna dodać pustych produktu osób");

                var url = _url.AddPersonProduct.Replace(Route.ID, productId.ToString()); ;
                var response = await _duHttpClient.PostAsync(url, personProduct);
                await EnsureCorrectResponse(response, "Błąd w czasie dodawania produktu osób");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania produktu osób: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania produktu osób: {Message}", ex.Message);
                throw;
            }
        }

        public async Task RemovePersonProduct(int personProductId)
        {
            try
            {
                if (personProductId == null)
                    throw new InvalidOperationException("Nie mozna usunąć produktu osób które nie posiada id");

                var url = _url.RemovePersonProduct.Replace(Route.ID, personProductId.ToString());
                var response = await _duHttpClient.DeleteAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu osób");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu osób: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu osób: {Message}", ex.Message);
                throw;
            }
        }

        public async Task ChangePersonPersonProduct(int personProductId, int personId)
        {
            try
            {
                if (personProductId == null)
                    throw new InvalidOperationException("Nie mozna zmienić osoby nie posiadającego id person produktu");
                if (personId == null)
                    throw new InvalidOperationException("Nie mozna zmienić osoby nie posiadającego id person osoby");

                var url = _url.ChangePersonPersonProduct
                    .Replace(Route.ID, personProductId.ToString())
                    .Replace(Route.PersonId, personId.ToString());
                var response = await _duHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu osoby");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu osoby: {Message}", ex.Message);
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
                if (personProductId == null)
                    throw new InvalidOperationException("Nie mozna rozliczyć produktu osoby nie posiadającego id");

                var url = _url.SetSettledPersonProduct
                    .Replace(Route.ID, personProductId.ToString())
                    .Replace(Route.Settled, settled.ToString());

                var response = await _duHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu osoby");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu osoby: {Message}", ex.Message);
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
                if (personProductId == null)
                    throw new InvalidOperationException("Nie mozna ustawić ceny produktu osoby nie posiadającego id");

                var data = new
                {
                    compensation = true
                };

                var url = _url.SetCompensationPersonProduct.Replace(Route.ID, personProductId.ToString());
                var response = await _duHttpClient.PutAsync(url, data);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji produktu");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji produktu osoby: {Message}", ex.Message);
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
                var url = _url.GetPersonProduct.Replace(Route.ID, personProductId.ToString());
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PersonProductDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktu osób");
                return result;
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
                var url = _url.GetPersonProductsForProduct.Replace(Route.ID, productId.ToString());
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<PersonProductDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktów osób");
                return result;
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
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<PersonProductDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktów osób");
                return result;
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
                throw new HttpResponseException(response.StatusCode, errorMessage);
            }
        }
    }
}
