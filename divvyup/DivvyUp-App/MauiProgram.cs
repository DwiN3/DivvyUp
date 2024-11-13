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
                httpClient.BaseAddress = new Uri("http://localhost:5185");
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

            builder.Services.AddTransient<IUserService, UserHttpService>();
            builder.Services.AddTransient<IPersonHttpService, PersonHttpService>();
            builder.Services.AddTransient<ILoanHttpService, LoanHttpSevice>();
            builder.Services.AddTransient<IReceiptService, ReceiptHttpService>();
            builder.Services.AddTransient<IProductService, ProductHttpService>();
            builder.Services.AddTransient<IPersonProductHttpService, PersonProductHttpService>();
            builder.Services.AddTransient<IChartService, ChartHttpService>();
            ;
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
