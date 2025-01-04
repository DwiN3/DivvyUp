using DivvyUp.Web.Data;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Request;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;

namespace DivvyUp.Web.Tests.IntegrationTests
{
    public class PersonControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly TestHelper _testHelper;
        private int userId;
        private string _userToken;

        public PersonControllerIntegrationTests(WebApplicationFactory<Program> factory)
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
            SetupTestEnvironmentAsync().GetAwaiter().GetResult();
        }

        private async Task SetupTestEnvironmentAsync()
        {
            await _testHelper.ClearDatabaseAsync();

            var (user, token) = await _testHelper.SetupUserWithTokenAsync(_client);
            _userToken = token;

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
            userId = dbContext.Users.First(u => u.Email == user.Email).Id;
            await dbContext.SaveChangesAsync();
        }

        [Theory]
        [InlineData("John", "Doe", true)]
        [InlineData("John", "", true)]
        [InlineData("", "Doe", false)]
        public async Task AddPerson_WithValidOrInvalidInput_ShouldBehaveAsExpected(string name, string surname, bool shouldSucceed)
        {
            // Arrange
            var addPersonRequest = new AddEditPersonDto { Name = name, Surname = surname };
            var requestMessage = _testHelper.CreateRequestWithToken(ApiRoute.PERSON_ROUTES.ADD, _userToken, HttpMethod.Put, addPersonRequest);

            // Act
            var addPersonResponse = await _client.SendAsync(requestMessage);

            // Assert
            if (shouldSucceed)
            {
                addPersonResponse.EnsureSuccessStatusCode();
                using (var scope = _factory.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
                    var persons = dbContext.Persons.Include(p => p.User).ToList();

                    Assert.Equal(2, persons.Count);

                    var addedPerson = persons.FirstOrDefault(p => p.Name == addPersonRequest.Name && p.Surname == addPersonRequest.Surname);
                    Assert.NotNull(addedPerson);
                    Assert.Equal(addPersonRequest.Name, addedPerson.Name);
                    Assert.Equal(addPersonRequest.Surname, addedPerson.Surname);
                    Assert.Equal(userId, addedPerson.UserId);
                }
            }
            else
            {
                Assert.Equal(HttpStatusCode.BadRequest, addPersonResponse.StatusCode);
                var responseContent = await addPersonResponse.Content.ReadAsStringAsync();
                Assert.Contains("Nazwa osoby jest wymagana", responseContent);
            }
        }
    }
}