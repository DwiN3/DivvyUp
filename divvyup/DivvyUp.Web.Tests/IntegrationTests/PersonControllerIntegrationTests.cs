using DivvyUp.Web.Data;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Request;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DivvyUp.Web.Tests.IntegrationTests
{
    public class PersonControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly TestHelper _testHelper;
        private RegisterUserDto _testUser;
        private string _userToken;

        public PersonControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DivvyUpDBContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }
                    services.AddLogging();

                    services.AddDbContext<DivvyUpDBContext>(options =>
                        options.UseInMemoryDatabase("TestDatabase"));

                    var serviceProvider = services.BuildServiceProvider();
                    var scopedServices = serviceProvider.CreateScope().ServiceProvider;
                    var db = scopedServices.GetRequiredService<DivvyUpDBContext>();
                    db.Database.EnsureCreated();
                });
            });

            _client = _factory.CreateClient();
            _testHelper = new TestHelper(_factory);
            SetupTestEnvironmentAsync().GetAwaiter().GetResult();
        }

        private async Task SetupTestEnvironmentAsync()
        {
            await _testHelper.ClearDatabaseAsync();
            _testUser = new RegisterUserDto
            {
                Username = "TestUserForPerson",
                Email = "testuser@example.com",
                Password = "TestPassword123",
            };

            await _testHelper.RegisterUserAsync(_client, _testUser);
            _userToken = await _testHelper.LoginAndGetTokenAsync(_client, _testUser.Username, _testUser.Password);
        }

        [Fact]
        public async Task AddPerson_ShouldAddNewPerson()
        {
            // Arrange
            var addPersonRequest = new AddEditPersonDto
            {
                Name = "John",
                Surname = "Doe"
            };

            var requestMessage = _testHelper.CreateRequestWithToken(ApiRoute.PERSON_ROUTES.ADD, _userToken, HttpMethod.Post, addPersonRequest);

            // Act
            var addPersonResponse = await _client.SendAsync(requestMessage);

            // Assert
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
                Assert.Equal(_testUser.Username, addedPerson.User.Username);
            }
        }

        [Fact]
        public async Task AddPersonWithoutName_ShouldReturnBadRequest()
        {
            // Arrange
            var addPersonRequest = new AddEditPersonDto
            {
                Name = ""
            };
            var requestMessage = _testHelper.CreateRequestWithToken(ApiRoute.PERSON_ROUTES.ADD, _userToken, HttpMethod.Post, addPersonRequest);

            // Act
            var addPersonResponse = await _client.SendAsync(requestMessage);

            // Assert
            Assert.Equal(400, (int)addPersonResponse.StatusCode);
            var responseContent = await addPersonResponse.Content.ReadAsStringAsync();
            Assert.Contains("Nazwa osoby jest wymagana", responseContent);
        }
    }
}