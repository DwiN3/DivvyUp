using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DivvyUp_Impl_Maui.Api.DHttpClient
{
    public class DHttpClient
    {
        private readonly HttpClient _httpClient;
        private string _token;

        public DHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            AddAuthorizationHeader();
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string url, T data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync(url, content);
        }

        public async Task<HttpResponseMessage> PutAsync(string url)
        {
            return await _httpClient.PutAsync(url, null);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _httpClient.DeleteAsync(url);
        }

        public void setToken(string token)
        {
            _token = token;
            AddAuthorizationHeader();
        }

        private void AddAuthorizationHeader()
        {
            if (_token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }
        }
    }
}