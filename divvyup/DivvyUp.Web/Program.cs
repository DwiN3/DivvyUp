using System.Text;
using DivvyUp.Web.Data;
using DivvyUp.Web.Interfac;
using DivvyUp.Web.Interface;
using DivvyUp.Web.Mappers;
using DivvyUp.Web.Middleware;
using DivvyUp.Web.Service;
using DivvyUp.Web.Update;
using DivvyUp.Web.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DivvyUp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddScoped<UserContext>();
            builder.Services.AddScoped<MyValidator>();
            builder.Services.AddScoped<EntityUpdateService>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPersonService, PersonService>();
            builder.Services.AddScoped<IReceiptService, ReceiptService>();
            builder.Services.AddScoped<ILoanService, LoanService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IPersonProductService, PersonProductService>();
            builder.Services.AddScoped<IChartService, ChartService>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddTransient<ExceptionMiddleware>();
            builder.Services.AddTransient<BearerTokenMiddleware>();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please enter the token into the field",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
            });


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<BearerTokenMiddleware>();

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
