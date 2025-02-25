using DivvyUp.Web.Configuration;
using DivvyUp.Web.Data;
using DivvyUp.Web.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace DivvyUp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Do wygenerowania pliku exe
            /*
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
               {
                  EnvironmentName = "Development" 
               });
            */
            builder.Services.AddServices();
            builder.Services.AddMapper();
            builder.Services.AddSwaggerGenConfiguration();
            builder.Services.AddAuthenticationServices(builder.Configuration);

            if (builder.Environment.IsEnvironment("Testing"))
            {
                builder.Services.AddDbContext<DivvyUpDBContext>(options =>
                    options.UseInMemoryDatabase("TestDatabase"));
            }
            else
            {
                builder.Services.AddDbContext<DivvyUpDBContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));
            }

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}