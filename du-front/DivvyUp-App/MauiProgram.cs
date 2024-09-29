using Microsoft.Extensions.Logging;
using Blazored.LocalStorage;
using DivvyUp_Impl_Maui.Api.DuHttpClient;
using DivvyUp_Impl_Maui.CodeReader;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Interface;
using Radzen;

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

            builder.Services.AddSingleton<DuHttpClient>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                return new DuHttpClient(httpClient);
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
            builder.Services.AddSingleton<DDialogService>();
            builder.Services.AddSingleton<DAlertService>();
            builder.Services.AddScoped<HeaderService>();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<IReceiptService, ReceiptService>();
            builder.Services.AddTransient<IPersonService, PersonService>();
            
            ;
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
