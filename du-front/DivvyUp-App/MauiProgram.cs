using DivvyUp_Web.Api.Interface;
using DivvyUp_Web.Api.Service;
using DivvyUp_Web.Api.Urls;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using DivvyUp_Web.Api.ResponceCodeReader;


namespace Receipt_Manager_App
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

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddFluentUIComponents();
            builder.Services.AddBlazorBootstrap();
            builder.Services.AddHttpClient();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddSingleton<Url>();
            builder.Services.AddSingleton<ResponseCodeReader>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
