using DivvyUp.Web.Data;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Request;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DivvyUp_Shared.Models;

namespace DivvyUp.Web.Tests.IntegrationTests
{
    public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly TestHelper _testHelper;
        private int userId;
        private string _userToken;
        private Receipt _receiptTest;

        public ProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
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
        }

        [Theory]
        [InlineData(10, true)]
        [InlineData(-10, false)]
        public async Task AddProduct_WithValidOrInvalidInput_ShouldBehaveAsExpected(decimal price, bool shouldSucceed)
        {
            // Arrange
            var addProductRequest = new AddEditProductDto() { Name = "TestProduct", Price = price };
            var url = ApiRoute.PRODUCT_ROUTES.ADD
                .Replace(ApiRoute.ARG_RECEIPT, _receiptTest.Id.ToString());
            var requestMessage = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Put, addProductRequest);

            // Act
            var addProductResponse = await _client.SendAsync(requestMessage);

            // Assert
            if (shouldSucceed)
            {
                addProductResponse.EnsureSuccessStatusCode();
                using (var scope = _factory.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
                    var product = dbContext.Products
                        .Include(p => p.Receipt)
                        .Where(p => p.Name.Equals(addProductRequest.Name))
                        .FirstOrDefault();

                    Assert.NotNull(product);
                    Assert.Equal(product.Name, addProductRequest.Name);
                    Assert.Equal(product.Price, addProductRequest.Price);
                }
            }
            else
            {
                Assert.Equal(HttpStatusCode.BadRequest, addProductResponse.StatusCode);
                var responseContent = await addProductResponse.Content.ReadAsStringAsync();
                Assert.Contains("Cena nie może być ujemna", responseContent);
            }
        }

        [Fact]
        public async Task AddProduct_WithInvalidMaxQuantity_ShouldReturnBadRequest()
        {
            // Arrange
            var addProductRequest = new AddEditProductDto() { Name = "TestProduct", Price = 10, Divisible = false, MaxQuantity = 2 };
            var url = ApiRoute.PRODUCT_ROUTES.ADD
                .Replace(ApiRoute.ARG_RECEIPT, _receiptTest.Id.ToString());
            var requestMessage = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Put, addProductRequest);

            // Act
            var addProductResponse = await _client.SendAsync(requestMessage);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, addProductResponse.StatusCode);
            var responseContent = await addProductResponse.Content.ReadAsStringAsync();
            Assert.Contains("Maksymalna liczba podzielności produktu musi być równa 1 gdy produkt jest niepodzielny", responseContent);
        }

        [Fact]
        public async Task AddProduct_WithInvalidDiscountPercentage_ShouldReturnBadRequest()
        {
            // Arrange
            var addProductRequest = new AddEditProductDto() { Name = "TestProduct", Price = 10, DiscountPercentage = 110};
            var url = ApiRoute.PRODUCT_ROUTES.ADD
                .Replace(ApiRoute.ARG_RECEIPT, _receiptTest.Id.ToString());
            var requestMessage = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Put, addProductRequest);

            // Act
            var addProductResponse = await _client.SendAsync(requestMessage);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, addProductResponse.StatusCode);
            var responseContent = await addProductResponse.Content.ReadAsStringAsync();
            Assert.Contains("Wartość procentowa jest błędnie ustawiona", responseContent);
        }

        [Fact]
        public async Task AddProduct_WithInvalidPurchasedQuantity_ShouldReturnBadRequest()
        {
            // Arrange
            var addProductRequest = new AddEditProductDto() { Name = "TestProduct", Price = 10, PurchasedQuantity = -2 };
            var url = ApiRoute.PRODUCT_ROUTES.ADD
                .Replace(ApiRoute.ARG_RECEIPT, _receiptTest.Id.ToString());
            var requestMessage = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Put, addProductRequest);

            // Act
            var addProductResponse = await _client.SendAsync(requestMessage);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, addProductResponse.StatusCode);
            var responseContent = await addProductResponse.Content.ReadAsStringAsync();
            Assert.Contains("Liczba sztuk produktu nie może być ujemna", responseContent);
        }

        [Fact]
        public async Task AddProduct_CalculateTotalPrice_ShouldCalculateCorrectly()
        {
            // Arrange
            var addProductRequest = new AddEditProductDto()
            {
                Name = "TestProduct", 
                Price = 25,
                PurchasedQuantity = 4, 
                DiscountPercentage = 15,
                AdditionalPrice = 2.50m
            };
            var url = ApiRoute.PRODUCT_ROUTES.ADD
                .Replace(ApiRoute.ARG_RECEIPT, _receiptTest.Id.ToString());
            var requestMessage = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Put, addProductRequest);

            // Act
            var addProductResponse = await _client.SendAsync(requestMessage);

            // Assert
            addProductResponse.EnsureSuccessStatusCode();
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
                var product = dbContext.Products
                    .Include(p => p.Receipt)
                    .Where(p => p.Name.Equals(addProductRequest.Name))
                    .FirstOrDefault();

                Assert.NotNull(product);
                Assert.Equal(addProductRequest.Name, product.Name);
                Assert.Equal(addProductRequest.Price, product.Price);
                Assert.Equal(addProductRequest.AdditionalPrice, product.AdditionalPrice);
                Assert.Equal(addProductRequest.PurchasedQuantity, product.PurchasedQuantity);
                Assert.Equal(addProductRequest.DiscountPercentage, product.DiscountPercentage);
                Assert.Equal(87.50m , product.TotalPrice);
            }
        }
    }
}
