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
        private Product _productTest2;
        private Product _productTest3;
        private PersonProduct _personProduct;
        private PersonProduct _personProduct2;

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
            _productTest2 = DataFactory.CreateProduct(_receiptTest.Id, "TestProduct2", 15.0m,  5);
            _productTest3 = DataFactory.CreateProduct(_receiptTest.Id, "TestProduct3", 15.97m, 3);
            dbContext.Products.AddRange(_productTest, _productTest2, _productTest3);
            await dbContext.SaveChangesAsync();

            _personTest = DataFactory.CreatePerson("TestPerson", userId);
            _personTest2 = DataFactory.CreatePerson("TestPerson2", userId);
            dbContext.Persons.AddRange(_personTest, _personTest2);
            await dbContext.SaveChangesAsync();

            _personProduct = DataFactory.CreatePersonProduct(_personTest.Id, _productTest3.Id, 1);
            _personProduct2 = DataFactory.CreatePersonProduct(_personTest2.Id, _productTest3.Id, 2);
            dbContext.PersonProducts.AddRange(_personProduct, _personProduct2);
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task AddPersonProduct_WithValidData_ShouldSucceed()
        {
            // Arrange
            var addRequest = new AddEditPersonProductDto { PersonId = _personTest.Id, Quantity = 1 };
            var url = ApiRoute.PERSON_PRODUCT_ROUTES.ADD.Replace(ApiRoute.ARG_PRODUCT, _productTest.Id.ToString());
            var request = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Put, addRequest);

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
            var request = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Put, addRequest);

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

        [Fact]
        public async Task CalculatePartPrice_WithValidData_ShouldSucceed()
        {
            // Arrange
            var addRequest1 = new AddEditPersonProductDto { PersonId = _personTest.Id, Quantity = 2 };
            var addRequest2 = new AddEditPersonProductDto { PersonId = _personTest2.Id, Quantity = 3 };

            var url = ApiRoute.PERSON_PRODUCT_ROUTES.ADD.Replace(ApiRoute.ARG_PRODUCT, _productTest2.Id.ToString());
            var request1 = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Put, addRequest1);
            var request2 = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Put, addRequest2);

            // Act
            var response1 = await _client.SendAsync(request1);
            var response2 = await _client.SendAsync(request2);

            // Assert
            response1.EnsureSuccessStatusCode();
            response2.EnsureSuccessStatusCode();

            await response1.Content.ReadAsStringAsync();
            await response2.Content.ReadAsStringAsync();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();

                var personProduct1 = dbContext.PersonProducts
                    .FirstOrDefault(pp => pp.PersonId == _personTest.Id && pp.ProductId == _productTest2.Id);
                var personProduct2 = dbContext.PersonProducts
                    .FirstOrDefault(pp => pp.PersonId == _personTest2.Id && pp.ProductId == _productTest2.Id);

                Assert.NotNull(personProduct1);
                Assert.NotNull(personProduct2);
                Assert.Equal(6.00m, personProduct1.PartOfPrice);
                Assert.Equal(9.00m, personProduct2.PartOfPrice);
            }
        }

        [Fact]
        public async Task AutoCompensation_WithValidData_ShouldSucceed()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();

                _personTest.CompensationAmount = 0.00m;
                _personTest2.CompensationAmount = 0.01m;
                dbContext.Persons.UpdateRange(_personTest, _personTest2);

                _productTest3.CompensationPrice = 0.01m;
                dbContext.Products.Update(_productTest3);
                await dbContext.SaveChangesAsync();
            }
            var url = ApiRoute.PERSON_PRODUCT_ROUTES.SET_AUTO_COMPENSATION
                .Replace(ApiRoute.ARG_PRODUCT, _productTest3.Id.ToString());
            var request = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Patch);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
                var personProduct1 = dbContext.PersonProducts
                    .Include(p => p.Person)
                    .FirstOrDefault(pp => pp.PersonId == _personTest.Id && pp.ProductId == _productTest3.Id);

                var personProduct2 = dbContext.PersonProducts
                    .Include(p => p.Person)
                    .FirstOrDefault(pp => pp.PersonId == _personTest2.Id && pp.ProductId == _productTest3.Id);

                Assert.NotNull(personProduct1);
                Assert.NotNull(personProduct2);
                Assert.Equal(true, personProduct1.Compensation);
                Assert.Equal(false, personProduct2.Compensation);
                Assert.Equal(0.01m, personProduct1.Person.CompensationAmount);
                Assert.Equal(0.00m, personProduct2.Person.CompensationAmount);
            }
        }
    }
}