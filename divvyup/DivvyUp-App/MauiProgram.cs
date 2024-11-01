using Microsoft.Extensions.Logging;
using Blazored.LocalStorage;
using DivvyUp_App.GuiService;
using DivvyUp_Impl_Maui.Service;
using DivvyUp_Shared.Interface;
using Radzen;
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

            builder.Services.AddSingleton<UserAppService>();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<DDialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<DNotificationService>();
            builder.Services.AddSingleton<DAlertService>();
            builder.Services.AddScoped<HeaderService>();

            builder.Services.AddTransient<IUserHttpService, UserHttpService>();
            builder.Services.AddTransient<IPersonHttpService, PersonHttpService>();
            builder.Services.AddTransient<ILoanHttpService, LoanHttpSevice>();
            builder.Services.AddTransient<IReceiptHttpService, ReceiptHttpService>();
            builder.Services.AddTransient<IProductHttpService, ProductHttpService>();
            builder.Services.AddTransient<IPersonProductHttpService, PersonProductHttpService>();
            builder.Services.AddTransient<IChartHttpService, ChartHttpService>();
            ;
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
