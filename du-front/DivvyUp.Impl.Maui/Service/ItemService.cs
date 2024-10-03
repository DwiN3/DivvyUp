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
    public class ItemService : IItemService
    {
        [Inject]
        private DuHttpClient _duHttpClient { get; set; }
        private Route _url { get; } = new();
        private readonly ILogger<ItemService> _logger;

        public ItemService(DuHttpClient duHttpClient, ILogger<ItemService> logger)
        {
            _duHttpClient = duHttpClient;
            _logger = logger;
        }

        public async Task AddItem(ItemDto item)
        {
            try
            {
                if (item == null)
                    throw new InvalidOperationException("Nie mozna dodać pustego produktu");
                if (item.name.Equals(string.Empty))
                    throw new InvalidOperationException("Nie mozna dodać productu bez nazwy");

                var url = _url.AddItem.Replace(Route.ID, item.receiptId.ToString());
                var response = await _duHttpClient.PostAsync(url, item);
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

        public async Task EditItem(ItemDto item)
        {
            try
            {
                if (item == null)
                    throw new InvalidOperationException("Nie mozna edytować pustego produktu");
                if (item.name.Equals(string.Empty))
                    throw new InvalidOperationException("Nie mozna edytować productu bez nazwy");

                var url = _url.EditItem.Replace(Route.ID, item.id.ToString());
                var response = await _duHttpClient.PutAsync(url, item);
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

        public async Task RemoveItem(int id)
        {
            try
            {
                if (id == null)
                    throw new InvalidOperationException("Nie mozna usunąć produktu które nie posiada id");

                var url = _url.RemoveItem.Replace(Route.ID, id.ToString());
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

        public async Task SetSettledItem(int id, bool isSettled)
        {
            try
            {
                if (id == null)
                    throw new InvalidOperationException("Nie mozna rozliczyć produktu nie posiadającego id");

                var data = new
                {
                    settled = isSettled
                };

                var url = _url.SetSettledItem.Replace(Route.ID, id.ToString());
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

        public async Task SetCompensationPriceItem(int id, double compensationPrice)
        {
            try
            {
                if (id == null)
                    throw new InvalidOperationException("Nie mozna ustawić ceny produktu nie posiadającego id");

                var data = new
                {
                    compensationPrice = compensationPrice
                };

                var url = _url.SetCompensationPriceItem.Replace(Route.ID, id.ToString());
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

        public async Task<ItemDto> GetItem(int itemId)
        {
            try
            {
                var url = _url.GetItem.Replace(Route.ID, itemId.ToString());
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ItemDto>(jsonResponse);
                await EnsureCorrectResponse(response, "Błąd w czasie pobieranie produktu");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd w czasie pobierania listy produktu do tabeli: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<ItemDto>> GetItems(int receiptId)
        {
            try
            {
                var url = _url.GetItems.Replace(Route.ID, receiptId.ToString());
                var response = await _duHttpClient.GetAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ItemDto>>(jsonResponse);
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
