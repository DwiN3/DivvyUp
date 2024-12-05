using System.Net;
using DivvyUp.Web.Data;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Request;
using DivvyUp_Shared.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DivvyUp.Web.Tests.IntegrationTests
{
    public class PersonProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly TestHelper _testHelper;
        private string _userToken;
        private Receipt _receiptTest;
        private Person _personTest;
        private Person _personTest2;
        private Product _productTest;

        public PersonProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
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
            var userId = dbContext.Users.First(u => u.Email == user.Email).Id;

            _receiptTest = DataFactory.CreateReceipt(userId, "TestReceipt", 20.0m);
            dbContext.Receipts.Add(_receiptTest);
            await dbContext.SaveChangesAsync();

            _productTest = DataFactory.CreateProduct(_receiptTest.Id, "TestProduct", 10.0m, 2);
            dbContext.Products.Add(_productTest);
            await dbContext.SaveChangesAsync();

            _personTest = DataFactory.CreatePerson("TestPerson", userId);
            _personTest2 = DataFactory.CreatePerson("TestPerson2", userId);
            dbContext.Persons.AddRange(_personTest, _personTest2);
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task AddPersonProduct_WithValidData_ShouldSucceed()
        {
            // Arrange
            var addRequest = new AddEditPersonProductDto { PersonId = _personTest.Id, Quantity = 1 };
            var url = ApiRoute.PERSON_PRODUCT_ROUTES.ADD.Replace(ApiRoute.ARG_PRODUCT, _productTest.Id.ToString());
            var request = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Post, addRequest);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
                var addedPersonProduct = dbContext.PersonProducts
                    .FirstOrDefault(pp => pp.PersonId == _personTest.Id && pp.ProductId == _productTest.Id);

                Assert.NotNull(addedPersonProduct);
                Assert.Equal(addedPersonProduct.PersonId, addRequest.PersonId);
                Assert.Equal(addedPersonProduct.Quantity, addRequest.Quantity);
            }
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(3, false)]
        public async Task AddPersonProduct_QuantityValidation_ShouldBehaveAsExpected(int quantity, bool shouldSucceed)
        {
            // Arrange
            var personId = quantity == 1 ? _personTest.Id : _personTest2.Id;
            var addRequest = new AddEditPersonProductDto { PersonId = personId, Quantity = quantity };
            var url = ApiRoute.PERSON_PRODUCT_ROUTES.ADD.Replace(ApiRoute.ARG_PRODUCT, _productTest.Id.ToString());
            var request = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Post, addRequest);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            if (shouldSucceed)
            {
                response.EnsureSuccessStatusCode();
            }
            else
            {
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                Assert.Contains("Przekroczono maksymalną ilość produktu", responseContent);
            }
        }
    }
}