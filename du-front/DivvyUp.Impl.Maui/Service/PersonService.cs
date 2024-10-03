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
    public class PersonService : IPersonService
    {
        [Inject]
        private DuHttpClient _duHttpClient { get; set; }
        private Route _url { get; } = new();
        private readonly ILogger<PersonService> _logger;

        public PersonService(DuHttpClient duHttpClient, ILogger<PersonService> logger)
        {
            _duHttpClient = duHttpClient;
            _logger = logger;
        }
        public async Task AddPerson(PersonDto person)
        {
            try
            {
                if (person == null)
                    throw new InvalidOperationException("Nie mozna dodać osoby");
                if (person.name.Equals(string.Empty))
                    throw new InvalidOperationException("Nie mozna dodać osoby bez imienia");

                var url = _url.AddPerson;
                var response = await _duHttpClient.PostAsync(url, person);
                await EnsureCorrectResponse(response, "Błąd w czasie dodawania osoby");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania osoby: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie dodawania osoby: {Message}", ex.Message);
                throw;
            }
        }

        public async Task EditPerson(PersonDto person)
        {
            try
            {
                if (person == null)
                    throw new InvalidOperationException("Nie mozna edytować osoby");
                if (person.name.Equals(string.Empty))
                    throw new InvalidOperationException("Nie mozna dodać edytować bez imienia");

                var url = _url.EditPerson.Replace(Route.ID, person.id.ToString());
                var response = await _duHttpClient.PutAsync(url, person);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji osoby");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji osoby: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji osoby: {Message}", ex.Message);
                throw;
            }
        }

        public async Task RemovePerson(int personId)
        {
            try
            {
                if (personId == null)
                    throw new InvalidOperationException("Nie mozna usunąć osoby które nie posiada id");

                var url = _url.RemovePerson.Replace(Route.ID, personId.ToString());
                var response = await _duHttpClient.DeleteAsync(url);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji osoby");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji osoby: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji osoby: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetReceiptsCountsPerson(int personId, int receiptsCounts)
        {
            try
            {
                if (personId == null)
                    throw new InvalidOperationException("Nie mozna ustawić ilości rachunków nie posiadającego id");

                var data = new
                {
                    receiptsCounts = receiptsCounts
                };

                var url = _url.SetReceiptsCountsPerson.Replace(Route.ID, personId.ToString());
                var response = await _duHttpClient.PutAsync(url, data);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji ilości rachunków");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji ilości rachunków: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji ilości rachunków: {Message}", ex.Message);
                throw;
            }
        }

        public async Task SetTotalPurchaseAmountPerson(int personId, double totalAmount)
        {
            try
            {
                if (personId == null)
                    throw new InvalidOperationException("Nie mozna ustawić bilansu nie posiadającego id");

                var data = new
                {
                    totalAmount = totalAmount
                };

                var url = _url.SetTotalPurchaseAmountPerson.Replace(Route.ID, personId.ToString());
                var response = await _duHttpClient.PutAsync(url, data);
                await EnsureCorrectResponse(response, "Błąd w czasie edycji bilansu");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Błąd w czasie edycji bilansu: {Message}", ex.Message);
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
                var url = _url.GetPerson.Replace(Route.ID, personId.ToString());
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PersonDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie osoby");
                return result;
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
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<PersonDto>>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie osób");
                return result;
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
                throw new HttpResponseException(response.StatusCode, errorMessage);
            }
        }
    }
}
