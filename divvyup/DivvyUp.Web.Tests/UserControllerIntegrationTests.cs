using DivvyUp.Web.Data;
using DivvyUp_Shared.Dtos.Request;
using Microsoft.AspNetCore.Mvc.Testing;
using DivvyUp_Shared.AppConstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace DivvyUp.Web.Tests
{
    public class UserControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly TestHelper _testHelper;

        public UserControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");

                builder.ConfigureServices(services =>
                {
                    services.AddLogging();

                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<DuDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<DuDbContext>(options =>
                        options.UseInMemoryDatabase("TestDatabase"));

                    var serviceProvider = services.BuildServiceProvider();
                    using var scope = serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<DuDbContext>();
                    db.Database.EnsureCreated();
                });
            });

            _client = _factory.CreateClient();
            _testHelper = new TestHelper(_factory);
        }

        [Fact]
        public async Task Register_NewUser_ShouldAddUserToDatabase()
        {
            // Arrange
            await _testHelper.ClearDatabaseAsync();

            var newUser = new RegisterUserDto
            {
                Username = "DBTest",
                Email = "dbtestuser@example.com",
                Password = "TestPassword123",
            };

            // Act
            var content = _testHelper.CreateJsonContent(newUser);
            var response = await _client.PostAsync(ApiRoute.USER_ROUTES.REGISTER, content);

            // Assert
            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DuDbContext>();
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);

                Assert.NotNull(user);
                Assert.Equal(newUser.Username, user.Username);
            }
        }

        [Fact]
        public async Task Register_User_WithDuplicateUsername_ShouldReturnConflictStatusCode()
        {
            // Arrange
            await _testHelper.ClearDatabaseAsync();

            var existingUser = new RegisterUserDto
            {
                Username = "DBTest",
                Email = "dbtestuser@example.com",
                Password = "TestPassword123"
            };

            // Act
            var content = _testHelper.CreateJsonContent(existingUser);
            var firstResponse = await _client.PostAsync(ApiRoute.USER_ROUTES.REGISTER, content);
            firstResponse.EnsureSuccessStatusCode();

            var duplicateUser = new
            {
                Username = "DBTest",
                Email = "dbtestuser2@example.com",
                Password = "AnotherPassword123",
            };

            var duplicateContent = _testHelper.CreateJsonContent(duplicateUser);
            var duplicateResponse = await _client.PostAsync(ApiRoute.USER_ROUTES.REGISTER, duplicateContent);
            
            // Assert
            Assert.Equal(409, (int)duplicateResponse.StatusCode);
            var responseContent = await duplicateResponse.Content.ReadAsStringAsync();
            Assert.Contains("Użytkownik o takich danych istnieje", responseContent);
        }
    }
}