using DivvyUp.Web.Data;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Request;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            var addProductRequest = new AddEditProductDto() { Name = "TestProduct", Price = price, Divisible = false, MaxQuantity = 1 };
            var url = ApiRoute.PRODUCT_ROUTES.ADD
                .Replace(ApiRoute.ARG_RECEIPT, _receiptTest.Id.ToString());
            var requestMessage = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Post, addProductRequest);

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
                    Assert.Equal(product.Divisible, addProductRequest.Divisible);
                    Assert.Equal(product.MaxQuantity, addProductRequest.MaxQuantity);
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
            var requestMessage = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Post, addProductRequest);

            // Act
            var addProductResponse = await _client.SendAsync(requestMessage);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, addProductResponse.StatusCode);
            var responseContent = await addProductResponse.Content.ReadAsStringAsync();
            Assert.Contains("Maksymalna ilość musi być równa 1 gdy produkt jest niepodzielny", responseContent);
        }
    }
}
