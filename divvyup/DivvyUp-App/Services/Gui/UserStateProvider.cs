﻿using Blazored.LocalStorage;
using DivvyUp_Shared.HttpClients;
using System.Net.Http.Headers;

namespace DivvyUp_App.Services.Gui
{
    public class UserStateProvider
    {
        private const string UserTokenKey = "UserToken";
        private readonly ILocalStorageService _localStorageService;
        private readonly DHttpClient _httpClient;
        public event Action OnUserStateChanged;
        public bool AfterValidation { get; set; } = false;

        public UserStateProvider(ILocalStorageService localStorageService, DHttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorageService.GetItemAsync<string>(UserTokenKey);
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorageService.SetItemAsync(UserTokenKey, token);
            UpdateHttpClientHeader(token);
            AfterValidation = true;
            NotifyUserStateChanged();
        }

        public async Task ClearTokenAsync()
        {
            await _localStorageService.RemoveItemAsync(UserTokenKey);
            UpdateHttpClientHeader(string.Empty);
            AfterValidation = true;
            NotifyUserStateChanged();
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            return AfterValidation;
        }


        private void UpdateHttpClientHeader(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient._httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        private void NotifyUserStateChanged() => OnUserStateChanged?.Invoke();
    }
}
