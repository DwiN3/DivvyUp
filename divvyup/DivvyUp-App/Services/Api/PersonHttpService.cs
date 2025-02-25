﻿using DivvyUp_Shared.AppConstants;
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
    public class PersonHttpService : IPersonService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private readonly ILogger<PersonHttpService> _logger;

        public PersonHttpService(DHttpClient dHttpClient, ILogger<PersonHttpService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }
        public async Task Add(AddEditPersonDto person)
        {
            try
            {
                var url = ApiRoute.PERSON_ROUTES.ADD;
                var response = await _dHttpClient.PutAsync(url, person);
                await EnsureCorrectResponse(response, "Błąd w czasie dodawania osoby");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania osoby: {Message}", ex.Message);
                throw;
            }
        }

        public async Task Edit(AddEditPersonDto person, int personId)
        {
            try
            {
                var url = ApiRoute.PERSON_ROUTES.EDIT
                    .Replace(ApiRoute.ARG_PERSON, personId.ToString());
                var response = await _dHttpClient.PatchAsync(url, person);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji osoby");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji osoby: {Message}", ex.Message);
                throw;
            }
        }

        public async Task Remove(int personId)
        {
            try
            {
                var url = ApiRoute.PERSON_ROUTES.REMOVE
                    .Replace(ApiRoute.ARG_PERSON, personId.ToString());
                var response = await _dHttpClient.DeleteAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji osoby");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji osoby: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<PersonDto> GetPerson(int personId)
        {
            try
            {
                var url = ApiRoute.PERSON_ROUTES.PERSON
                    .Replace(ApiRoute.ARG_PERSON, personId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PersonDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie osoby");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania osoby: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<PersonDto>> GetPersons()
        {
            try
            {
                var url = ApiRoute.PERSON_ROUTES.PEOPLE;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<PersonDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie osób");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy osób do tabeli: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<PersonDto> GetUserPerson()
        {
            try
            {
                var url = ApiRoute.PERSON_ROUTES.PERSON_USER;
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PersonDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie osoby");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania osoby: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<PersonDto>> GetPersonFromReceipt(int receiptId)
        {
            try
            {
                var url = ApiRoute.PERSON_ROUTES.PEOPLE_FROM_RECEIPT.Replace(ApiRoute.ARG_RECEIPT, receiptId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<PersonDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie osób");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy osób do tabeli: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<PersonDto>> GetPersonFromProduct(int productId)
        {
            try
            {
                var url = ApiRoute.PERSON_ROUTES.PEOPLE_FROM_PRODUCT.Replace(ApiRoute.ARG_PRODUCT, productId.ToString());
                var response = await _dHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<PersonDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie osób");
                return result;
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy osób do tabeli: {Message}", ex.Message);
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
