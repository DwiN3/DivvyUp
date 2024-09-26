using DivvyUp_Web.Api.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Blazored.LocalStorage;
using DivvyUp_Impl.Api.DuHttpClient;
using DivvyUp_Impl.Api.Mapper;
using DivvyUp_Impl.Api.Route;
using DivvyUp_Impl.CodeReader;
using DivvyUp_Impl.Service;
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
            builder.Services.AddFluentUIComponents();
            builder.Services.AddBlazorBootstrap();
            builder.Services.AddRadzenComponents();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddSingleton<Route>();
            builder.Services.AddSingleton<CodeReaderResponse>();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<IReceiptService, ReceiptService>();
            builder.Services.AddSingleton<UserAppService>();
            ;
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
