using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DivvyUp_Web.DivvyUpHttpClient
{
    public class DuHttpClient
    { 
        public readonly HttpClient httpClient = new HttpClient();
        private string Token { get; set; }

        public DuHttpClient(string token)
        {
            Token = token;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            AddAuthorizationHeader();
            return await httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
        {
            AddAuthorizationHeader();

            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            return await httpClient.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string url, T data)
        {
            AddAuthorizationHeader();

            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            return await httpClient.PutAsync(url, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            AddAuthorizationHeader();
            return await httpClient.DeleteAsync(url);
        }

        private void AddAuthorizationHeader()
        {
            System.Diagnostics.Debug.Print("Token w HTTP :"+Token);
            if (Token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }
        }
    }
}
