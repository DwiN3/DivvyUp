using Microsoft.Extensions.Logging;
using Blazored.LocalStorage;
using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Interface;
using Radzen;
using DivvyUp_Impl_Maui.Api.CodeReader;
using DivvyUp_Impl_Maui.Api.DHttpClient;

namespace DivvyUp_App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSingleton<DHttpClient>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                return new DHttpClient(httpClient);
            });

            
            builder.Services.AddHttpClient();
            builder.Services.AddMauiBlazorWebView();
            //builder.Services.AddFluentUIComponents();
            builder.Services.AddBlazorBootstrap();
            builder.Services.AddRadzenComponents();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddSingleton<CodeReaderResponse>();
            builder.Services.AddSingleton<UserAppService>();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<DDialogService>();
            builder.Services.AddSingleton<DAlertService>();
            builder.Services.AddScoped<HeaderService>();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<IPersonService, PersonService>();
            builder.Services.AddTransient<IReceiptService, ReceiptService>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<IPersonProductService, PersonProductService>();
            ;
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
