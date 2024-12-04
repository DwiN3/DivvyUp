using DivvyUp.Web.Data;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Entity;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DivvyUp.Web.Tests
{
    public class PersonProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly TestHelper _testHelper;
        private RegisterUserDto _testUser;
        private string _userToken;
        private Person _personTest;
        private Receipt _receipt;
        private Product _productTest;
        private User _userTest;

        public PersonProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    var descriptor =
                        services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DivvyUpDBContext>));
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
                Username = "TestUserForPersonProduct",
                Email = "testuser@example.com",
                Password = "TestPassword123",
            };

            await _testHelper.RegisterUserAsync(_client, _testUser);
            _userToken = await _testHelper.LoginAndGetTokenAsync(_client, _testUser.Username, _testUser.Password);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
                _userTest = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == _testUser.Email);

                _personTest = new Person
                {
                    UserId = 1,
                    Name = "TestUserForPersonProduct",
                    Surname = "",
                    ReceiptsCount = 1,
                    ProductsCount = 0,
                    TotalAmount = 0,
                    UnpaidAmount = 0,
                    CompensationAmount = 0,
                    LoanBalance = 0,
                    UserAccount = true
                };

                _receipt = new Receipt
                {
                    UserId = 1,
                    Name = "TestReceipt",
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    TotalPrice = 15.99m,
                    Settled = false
                };

                _productTest = new Product
                {
                    ReceiptId = 1,
                    Name = "TestProduct",
                    Price = 15.99m,
                    AdditionalPrice = 0,
                    Divisible = true,
                    MaxQuantity = 2,
                    AvailableQuantity = 2,
                    CompensationPrice = 15.99m,
                    Settled = false
                };

                dbContext.Persons.Add(_personTest);
                dbContext.Receipts.Add(_receipt);
                dbContext.Products.Add(_productTest);
                await dbContext.SaveChangesAsync();
            }
        }


        [Fact]
        public async Task AddPersonProduct_ShouldAddNewPersonProduct()
        {
            // Arrange
            var addPersonProductRequest = new AddEditPersonProductDto()
            {
                PersonId = 1,
                Quantity = 1
            };
            var url = ApiRoute.PERSON_PRODUCT_ROUTES.ADD.Replace(ApiRoute.ARG_PRODUCT, _productTest.Id.ToString());
            var requestMessage = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Post, addPersonProductRequest);

            // Act
            var addPersonResponse = await _client.SendAsync(requestMessage);

            // Assert
            addPersonResponse.EnsureSuccessStatusCode();
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
                var personProducts = dbContext.PersonProducts
                    .Include(p => p.Product)
                    .Include(p => p.Person)
                    .Where(p => p.Person.UserId == _userTest.Id && p.ProductId == _productTest.Id);

                Assert.Equal(1, personProducts.Count());
                var addedPersonResponse = personProducts.FirstOrDefault(p => p.PersonId == addPersonProductRequest.PersonId);
                Assert.NotNull(addedPersonResponse);
            }
        }
    }
}
