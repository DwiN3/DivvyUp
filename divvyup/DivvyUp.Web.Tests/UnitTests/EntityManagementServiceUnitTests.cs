using DivvyUp.Web.EntityManager;

namespace DivvyUp.Web.Tests.UnitTests
{
    public class EntityManagementServiceUnitTests
    {
        private readonly EntityManagementService _service;

        public EntityManagementServiceUnitTests()
        {
            _service = new EntityManagementService(null, null);
        }

        [Fact]
        public async Task CalculatePartOfPrice_ShouldReturnCorrectResult()
        {
            // Arrange
            int quantity = 2;
            int maxQuantity = 10;
            decimal price = 100m;

            // Act
            var result = await _service.CalculatePartOfPrice(quantity, maxQuantity, price);

            // Assert
            Assert.Equal(20.00m, result);
        }

        [Fact]
        public async Task CalculateTotalPrice_AddAditionalPrice_ShouldCalculateCorrectly()
        {
            // Arrange
            decimal price = 100.00m;
            int purchasedQuantity = 1;
            decimal additionalPrice = 25.00m;
            int discountPercentage = 0;

            // Act
            var result = _service.CalculateTotalPrice(price, purchasedQuantity, additionalPrice, discountPercentage);

            // Assert
            Assert.Equal(125.00m, result);
        }

        [Fact]
        public async Task CalculateTotalPrice_ChangePurchasedQuantity_ShouldCalculateCorrectly()
        {
            // Arrange
            decimal price = 70.50m;
            int purchasedQuantity = 3;
            decimal additionalPrice = 0.00m;
            int discountPercentage = 0;

            // Act
            var result = _service.CalculateTotalPrice(price, purchasedQuantity, additionalPrice, discountPercentage);

            // Assert
            Assert.Equal(211.50m, result);
        }

        [Fact]
        public async Task CalculateTotalPrice_ChangeDiscountPercentage_ShouldCalculateCorrectly()
        {
            // Arrange
            decimal price = 80.00m;
            int purchasedQuantity = 1;
            decimal additionalPrice = 0.00m;
            int discountPercentage = 10;

            // Act
            var result = _service.CalculateTotalPrice(price, purchasedQuantity, additionalPrice, discountPercentage);

            // Assert
            Assert.Equal(72.00m, result);
        }

        [Fact]
        public async Task CalculateTotalPrice_ShouldCalculateCorrectly()
        {
            // Arrange
            decimal price = 10.00m;
            int purchasedQuantity = 2;
            decimal additionalPrice = 0.50m;
            int discountPercentage = 10;

            // Act
            var result = _service.CalculateTotalPrice(price, purchasedQuantity, additionalPrice, discountPercentage);

            // Assert
            Assert.Equal(18.50m, result);
        }
    }
}