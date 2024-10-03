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

        public async Task AddProductPerson(PersonProductDto personProduct, int productId)
        {
            try
            {
                if (personProduct == null)
                    throw new InvalidOperationException("Nie mozna dodać pustych detali produktu");

                var url = _url.AddPersonProduct.Replace(Route.ID, productId.ToString()); ;
                var response = await _duHttpClient.PostAsync(url, personProduct);
                await EnsureCorrectResponse(response, "Błąd w czasie dodawania rachunku");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania rachunku: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania rachunku: {Message}", ex.Message);
                throw;
            }
        }

        public async Task RemoveProductPerson(int personProductId)
        {
            throw new NotImplementedException();
        }

        public async Task SetSettledProductPerson(int personProductId, bool isSettled)
        {
            throw new NotImplementedException();
        }

        public async Task SetCompensation(int personProductId, bool isCompensation)
        {
            throw new NotImplementedException();
        }

        public async Task<PersonProductDto> GetPersonProduct(int personProductId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PersonProductDto>> GetPersonProducts(int productId)
        {
            try
            {
                var url = _url.GetPersonProducts.Replace(Route.ID, productId.ToString());
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<PersonProductDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie rachunków");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy rachunków do tabeli: {Message}", ex.Message);
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
