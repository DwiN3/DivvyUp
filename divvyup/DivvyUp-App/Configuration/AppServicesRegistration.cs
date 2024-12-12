using Blazored.LocalStorage;
using DivvyUp_App.Services.Api;
using DivvyUp_App.Services.Gui;
using DivvyUp_Shared.Interfaces;
using Radzen;

namespace DivvyUp_App.Configuration
{
    public static class AppServicesRegistration
    {
        public static void RegisterAppServices(this IServiceCollection services)
        {
            services.AddMauiBlazorWebView();
            services.AddBlazorBootstrap();
            services.AddRadzenComponents();
            services.AddBlazoredLocalStorage();

            services.AddTransient<IUserService, UserHttpService>();
            services.AddTransient<IPersonService, PersonHttpService>();
            services.AddTransient<ILoanService, LoanHttpSevice>();
            services.AddTransient<IReceiptService, ReceiptHttpService>();
            services.AddTransient<IProductService, ProductHttpService>();
            services.AddTransient<IPersonProductService, PersonProductHttpService>();
            services.AddTransient<IChartService, ChartHttpService>();

            services.AddScoped<DialogService>();
            services.AddScoped<DDialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<DNotificationService>();
            services.AddSingleton<DAlertService>();
            services.AddScoped<HeaderService>();
            services.AddScoped<UserStateProvider>();
        }
    }
}
