﻿using DivvyUp_Impl_Maui.Api.DHttpClient;
using DivvyUp_Impl_Maui.Api.Exceptions;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dto;
using DivvyUp_Shared.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DivvyUp_Impl_Maui.Service
{
    public class PersonService : IPersonService
    {
        [Inject]
        private DHttpClient _dHttpClient { get; set; }
        private ApiRoute _url { get; } = new();
        private readonly ILogger<PersonService> _logger;

        public PersonService(DHttpClient dHttpClient, ILogger<PersonService> logger)
        {
            _dHttpClient = dHttpClient;
            _logger = logger;
        }
        public async Task Add(PersonDto person)
        {
            try
            {
                var url = _url.AddPerson;
                var response = await _dHttpClient.PostAsync(url, person);
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

        public async Task Edit(PersonDto person)
        {
            try
            {
                var url = _url.EditPerson.Replace(ApiRoute.arg_ID, person.id.ToString());
                var response = await _dHttpClient.PutAsync(url, person);
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
                var url = _url.RemovePerson.Replace(ApiRoute.arg_ID, personId.ToString());
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

        public async Task SetReceiptsCounts(int personId, int receiptsCounts)
        {
            try
            {
                var url = _url.SetReceiptsCountsPerson
                    .Replace(ApiRoute.arg_ID, personId.ToString())
                    .Replace(ApiRoute.arg_ReceiptsCount, receiptsCounts.ToString());
                var response = await _dHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji ilości rachunków");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji ilości rachunków: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetTotalAmount(int personId, double totalAmount)
        {
            try
            {
                if (personId == null)
                    throw new InvalidOperationException("Nie mozna ustawić bilansu nie posiadającego id");

                var url = _url.SetTotalAmountPerson
                    .Replace(ApiRoute.arg_ID, personId.ToString())
                    .Replace(ApiRoute.arg_TotalAmount, totalAmount.ToString());
                var response = await _dHttpClient.PutAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji bilansu");
            }
            catch (DException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji bilansu: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<PersonDto> GetPerson(int personId)
        {
            try
            {
                var url = _url.GetPerson.Replace(ApiRoute.arg_ID, personId.ToString());
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
                var url = _url.GetPersons;
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
                var url = _url.GetUserPerson;
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
                var url = _url.GetPersonsFromReceipt.Replace(ApiRoute.arg_ID, receiptId.ToString());
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
                var url = _url.GetPersonsFromProduct.Replace(ApiRoute.arg_ID, productId.ToString());
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
                _logger.LogError("{ErrorMessage} Kod '{StatusCode}'. Response: '{Response}'", errorMessage, response.StatusCode, content);
                throw new DException(response.StatusCode, content);
            }
        }
    }
}
