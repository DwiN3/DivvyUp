using DivvyUp.Web.Data;
using DivvyUp.Web.Update;
using Microsoft.EntityFrameworkCore;

namespace DivvyUp.Web.Tests.UnitTests
{
    public class EntityUpdateServiceUnitTests
    {
        private readonly EntityUpdateService _updateService;
        private readonly DivvyUpDBContext _dbContext;

        public EntityUpdateServiceUnitTests()
        {
            var options = new DbContextOptionsBuilder<DivvyUpDBContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _dbContext = new DivvyUpDBContext(options);
            _updateService = new EntityUpdateService(_dbContext);
        }

        [Theory]
        [InlineData(2, 10, 100, 20.00)]
        [InlineData(1, 5, 50, 10.00)]
        [InlineData(3, 6, 60, 30.00)]
        [InlineData(0, 10, 100, 0.00)]
        public async Task CalculatePartOfPrice_ShouldReturnCorrectResult(int quantity, int maxQuantity, decimal price, decimal expectedResult)
        {
            // Act
            var result = await _updateService.CalculatePartOfPrice(quantity, maxQuantity, price);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
