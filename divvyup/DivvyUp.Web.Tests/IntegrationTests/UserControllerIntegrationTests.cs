using DivvyUp.Web.Data;
using DivvyUp_Shared.Dtos.Request;
using Microsoft.AspNetCore.Mvc.Testing;
using DivvyUp_Shared.AppConstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;

namespace DivvyUp.Web.Tests.IntegrationTests
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
                    services.RemoveAll(typeof(DbContextOptions<DivvyUpDBContext>));
                    services.AddDbContext<DivvyUpDBContext>(options => options.UseInMemoryDatabase("TestDatabase"));
                });
            });

            _client = _factory.CreateClient();
            _testHelper = new TestHelper(_factory);
        }

        [Fact]
        public async Task RegisterUser_WithValidData_ShouldSucceed()
        {
            // Arrange
            await _testHelper.ClearDatabaseAsync();

            var newUser = new RegisterUserDto
            {
                Username = "DBTest",
                Name = "DBTestName",
                Email = "dbtestuser@example.com",
                Password = "TestPassword123",
            };
            var request = _testHelper.CreateRequest(ApiRoute.USER_ROUTES.REGISTER, HttpMethod.Post, newUser);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);

                Assert.NotNull(user);
                Assert.Equal(newUser.Username, user.Username);
            }
        }

        [Fact]
        public async Task RegisterUser_WithDuplicateUsername_ShouldReturnConflict()
        {
            // Arrange
            await _testHelper.ClearDatabaseAsync();

            var existingUser = new RegisterUserDto
            {
                Username = "DBTest",
                Name = "DBTestName",
                Email = "dbtestuser@example.com",
                Password = "TestPassword123"
            };

            var duplicateUser = new
            {
                Username = "DBTest",
                Name = "DBTestName2",
                Email = "dbtestuser2@example.com",
                Password = "AnotherPassword123",
            };

            // Act
            var firstRequest = _testHelper.CreateRequest(ApiRoute.USER_ROUTES.REGISTER, HttpMethod.Post, existingUser);
            var firstResponse = await _client.SendAsync(firstRequest);

            var duplicateRequest = _testHelper.CreateRequest(ApiRoute.USER_ROUTES.REGISTER, HttpMethod.Post, duplicateUser);
            var duplicateResponse = await _client.SendAsync(duplicateRequest);

            // Assert
            firstResponse.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.Conflict, duplicateResponse.StatusCode);
            var responseContent = await duplicateResponse.Content.ReadAsStringAsync();
            Assert.Contains("Użytkownik o takich danych istnieje", responseContent);
        }
    }
}