using DivvyUp.Web.Data;
using DivvyUp.Web.Mappers;
using DivvyUp.Web.Middlewares;
using DivvyUp.Web.Services;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validation;
using DivvyUp_Shared.Interfaces;

namespace DivvyUp.Web.Configuration
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<UserContext>();
            services.AddScoped<DuValidator>();
            services.AddScoped<EntityUpdateService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPersonProductService, PersonProductService>();
            services.AddScoped<IChartService, ChartService>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddHttpContextAccessor();
            services.AddTransient<ExceptionMiddleware>();
        }
    }
}
