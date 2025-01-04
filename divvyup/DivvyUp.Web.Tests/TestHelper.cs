using DivvyUp.Web.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using DivvyUp_Shared.AppConstants;
using DivvyUp_Shared.Dtos.Request;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace DivvyUp.Web.Tests
{
    public class TestHelper
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TestHelper(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public async Task ClearDatabaseAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DivvyUpDBContext>();
            dbContext.PersonProducts.RemoveRange(dbContext.PersonProducts);
            dbContext.Products.RemoveRange(dbContext.Products);
            dbContext.Receipts.RemoveRange(dbContext.Receipts);
            dbContext.Loans.RemoveRange(dbContext.Loans);
            dbContext.Persons.RemoveRange(dbContext.Persons);
            dbContext.Users.RemoveRange(dbContext.Users);
            await dbContext.SaveChangesAsync();
        }

        public StringContent CreateJsonContent(object data)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            return new StringContent(serializedData, Encoding.UTF8, "application/json");
        }

        public async Task<(RegisterUserDto, string)> SetupUserWithTokenAsync(HttpClient client, string username = "TestUser", string email = "testuser@example.com", string password = "TestPassword123")
        {
            var userDto = new RegisterUserDto { Username = username, Name = "TestUserName", Email = email, Password = password };
            await RegisterUserAsync(client, userDto);
            var token = await LoginAndGetTokenAsync(client, username, password);
            return (userDto, token);
        }

        public async Task RegisterUserAsync(HttpClient client, RegisterUserDto registerUserDto)
        {
            var content = CreateJsonContent(registerUserDto);
            var response = await client.PostAsync(ApiRoute.USER_ROUTES.REGISTER, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<string> LoginAndGetTokenAsync(HttpClient client, string username, string password)
        {
            var loginRequest = new { Username = username, Password = password };
            var loginContent = CreateJsonContent(loginRequest);
            var response = await client.PostAsync(ApiRoute.USER_ROUTES.LOGIN, loginContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return responseContent.Trim('"');
        }

        public HttpRequestMessage CreateRequest(string url, HttpMethod method, object data = null)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = data != null ? CreateJsonContent(data) : null
            };
            return request;
        }

        public HttpRequestMessage CreateRequestWithToken(string url, string token, HttpMethod method, object data = null)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = data != null ? CreateJsonContent(data) : null
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return request;
        }
    }
}
