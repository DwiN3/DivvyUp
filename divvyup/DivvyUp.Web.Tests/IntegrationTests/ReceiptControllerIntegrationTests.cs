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
using Azure;

namespace DivvyUp.Web.Tests.IntegrationTests
{
    public class ReceiptControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly TestHelper _testHelper;
        private int userId;
        private string _userToken;
        private Receipt _receiptTest;
        private Product _productTest;

        public ReceiptControllerIntegrationTests(WebApplicationFactory<Program> factory)
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

            _receiptTest = DataFactory.CreateReceipt(userId, "TestReceipt", 20.0m);
            dbContext.Receipts.Add(_receiptTest);
            await dbContext.SaveChangesAsync();

            _productTest = DataFactory.CreateProduct(_receiptTest.Id, "TestProduct", 10.0m, 2);
            dbContext.Products.Add(_productTest);
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task SetSettledInReceipt_WithValid_ShouldBehaveAsExpected()
        {
            // Arrange
            bool settled = true;
            var url = ApiRoute.RECEIPT_ROUTES.SET_SETTLED
                .Replace(ApiRoute.ARG_RECEIPT, _receiptTest.Id.ToString())
                .Replace(ApiRoute.ARG_SETTLED, settled.ToString());
            var requestMessage = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Put);

            // Act
            var setSettledResponse = await _client.SendAsync(requestMessage);

            // Assert
            setSettledResponse.EnsureSuccessStatusCode();
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
                var receipt = dbContext.Receipts
                    .Where(p => p.Id == _receiptTest.Id)
                    .FirstOrDefault();

                var product = dbContext.Products
                    .Where(p => p.Id == _productTest.Id)
                    .FirstOrDefault();

                Assert.NotNull(receipt);
                Assert.NotNull(product);
                Assert.Equal(true, receipt.Settled);
                Assert.Equal(true, product.Settled);
            }
        }

        [Fact]
        public async Task GetReceipt_WithValid_ShouldReturnNotFound()
        {
            // Arrange
            int receiptId = -1;
            var url = ApiRoute.RECEIPT_ROUTES.RECEIPT
                .Replace(ApiRoute.ARG_RECEIPT, receiptId.ToString());
            var requestMessage = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Get);

            // Act
            var getReceiptResponse = await _client.SendAsync(requestMessage);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, getReceiptResponse.StatusCode);
            var responseContent = await getReceiptResponse.Content.ReadAsStringAsync();
            Assert.Contains("Rachunek nie znaleziony", responseContent);
        }

        [Fact]
        public async Task RemoveReceiptWithProduct_WithValid_ShouldBehaveAsExpected()
        {
            // Arrange
            var url = ApiRoute.RECEIPT_ROUTES.REMOVE
                .Replace(ApiRoute.ARG_RECEIPT, _receiptTest.Id.ToString());
            var requestMessage = _testHelper.CreateRequestWithToken(url, _userToken, HttpMethod.Delete);

            // Act
            var removeReceiptResponse = await _client.SendAsync(requestMessage);

            // Assert
            removeReceiptResponse.EnsureSuccessStatusCode();
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
                var receipt = dbContext.Receipts
                    .Where(p => p.Id == _receiptTest.Id)
                    .FirstOrDefault();

                var product = dbContext.Products
                    .Where(p => p.Id == _productTest.Id)
                    .FirstOrDefault();

                Assert.Null(receipt);
                Assert.Null(product);
            }
        }
    }
}
