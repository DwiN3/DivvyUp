using DivvyUp.Web.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

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
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DuDbContext>();
                dbContext.Users.RemoveRange(dbContext.Users);
                await dbContext.SaveChangesAsync();
            }
        }

        public StringContent CreateJsonContent(object data)
        {
            return new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json"
            );
        }
    }
}
