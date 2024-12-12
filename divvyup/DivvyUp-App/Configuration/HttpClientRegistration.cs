using DivvyUp_Shared.HttpClients;

namespace DivvyUp_App.Configuration
{
    public static class HttpClientRegistration
    {
        public static void RegisterHttpClient(this IServiceCollection services)
        {
            services.AddSingleton<DHttpClient>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri("http://localhost:5185");
                return new DHttpClient(httpClient);
            });
            services.AddHttpClient();
        }
    }
}
