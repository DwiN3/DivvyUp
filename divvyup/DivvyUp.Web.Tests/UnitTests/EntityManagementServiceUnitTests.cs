using DivvyUp.Web.Data;
using DivvyUp.Web.EntityManager;
using DivvyUp_Shared.Exceptions;
using DivvyUp_Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DivvyUp.Web.Tests.UnitTests
{
    public class EntityManagementServiceUnitTests
    {
        private readonly EntityManagementService _service;
        private readonly DivvyUpDBContext _dbContext;

        public EntityManagementServiceUnitTests()
        {
            var options = new DbContextOptionsBuilder<DivvyUpDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new DivvyUpDBContext(options);

            var userContext = new UserContext(null, _dbContext);
            _service = new EntityManagementService(_dbContext, userContext);
        }

        [Fact]
        public async Task GetReceipt_ShouldReturnReceipt_WhenValidUserAndReceiptId()
        {
            // Arrange
            var user = new User { Id = 1, Username = "TestUser", Name = "TestUserName", Surname = string.Empty, Email = "testuser@example.com", Password = "TestPassword123" };
            _dbContext.Users.Add(user);

            var receipt = new Receipt { Id = 1, UserId = user.Id, Name = "Test Receipt", TotalPrice = 100.00m };
            _dbContext.Receipts.Add(receipt);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _service.GetReceipt(user, receipt.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(receipt.Id, result.Id);
            Assert.Equal(receipt.Name, result.Name);
            Assert.Equal(receipt.TotalPrice, result.TotalPrice);
        }

        [Fact]
        public async Task GetReceipt_ShouldThrowNotFoundException_WhenReceiptDoesNotExist()
        {
            // Arrange
            var user = new User { Id = 1, Username = "TestUser", Name = "TestUserName", Surname = string.Empty, Email = "testuser@example.com", Password = "TestPassword123" };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Act && Assert
            var exception = await Assert.ThrowsAsync<DException>(() => _service.GetReceipt(user, 999));
            Assert.Equal(HttpStatusCode.NotFound, exception.Status);
            Assert.Equal("Rachunek nie znaleziony", exception.Message);
        }

        [Fact]
        public async Task GetReceipt_ShouldThrowUnauthorizedException_WhenUserDoesNotOwnReceipt()
        {
            // Arrange
            var user1 = new User { Id = 1, Username = "TestUser", Name = "TestUserName", Surname = string.Empty, Email = "testuser@example.com", Password = "TestPassword123" };
            var user2 = new User { Id = 2, Username = "TestUser2", Name = "TestUserName", Surname = string.Empty, Email = "testuser2@example.com", Password = "TestPassword123" };
            _dbContext.Users.AddRange(user1, user2);

            var receipt = new Receipt { Id = 1, UserId = user1.Id, Name = "Test Receipt", TotalPrice = 100.00m };
            _dbContext.Receipts.Add(receipt);
            await _dbContext.SaveChangesAsync();

            // Act && Assert
            var exception = await Assert.ThrowsAsync<DException>(() => _service.GetReceipt(user2, receipt.Id));
            Assert.Equal(HttpStatusCode.Unauthorized, exception.Status);
            Assert.Equal("Brak dostępu do rachunku: 1", exception.Message);
        }

        [Fact]
        public async Task GetPersonWithLowestCompensation_ShouldReturnPersonWithLowestCompensation()
        {
            // Arrange
            int productId = 1;
            
            var person1 = new Person { Id = 1, UserId = 1, Name = "Person 1", Surname = "", CompensationAmount = 10.00m, }; 
            var person2 = new Person { Id = 2, UserId = 2, Name = "Person 2", Surname = "", CompensationAmount = 5.00m, }; 
            var person3 = new Person { Id = 3, UserId = 3, Name = "Person 3", Surname = "", CompensationAmount = 7.00m,}; 
            _dbContext.Persons.AddRange(person1, person2, person3);

            var personProduct1 = new PersonProduct { PersonId = person1.Id, ProductId = productId };
            var personProduct2 = new PersonProduct { PersonId = person2.Id, ProductId = productId };
            var personProduct3 = new PersonProduct { PersonId = person3.Id, ProductId = productId };
            _dbContext.PersonProducts.AddRange(personProduct1, personProduct2, personProduct3);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _service.GetPersonWithLowestCompensation(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(person2.Id, result.PersonId);
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(false, true, true, false)]
        [InlineData(false, false, false, false)]
        public async Task AreAllPersonProductsSettled_ShouldReturnExpectedResult_WhenDifferentProductStatusesAreUsed(bool settled1, bool settled2, bool settled3, bool expected)
        {
            int productId = 2;

            var personProduct1 = new PersonProduct { PersonId = 1, ProductId = productId, Settled = settled1 };
            var personProduct2 = new PersonProduct { PersonId = 2, ProductId = productId, Settled = settled2 };
            var personProduct3 = new PersonProduct { PersonId = 3, ProductId = productId, Settled = settled3 };
            _dbContext.PersonProducts.AddRange(personProduct1, personProduct2, personProduct3);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _service.AreAllPersonProductsSettled(productId);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task UpdateCompensationFlags_ShouldUpdateCompensationFlagsCorrectly()
        {
            // Arrange
            int productId = 1;
            int person1Id = 1;
            int person2Id = 2;
            int person3Id = 3;

            var personProduct1 = new PersonProduct { PersonId = person1Id, ProductId = productId, Compensation = true };
            var personProduct2 = new PersonProduct { PersonId = person2Id, ProductId = productId, Compensation = false };
            var personProduct3 = new PersonProduct { PersonId = person3Id, ProductId = productId, Compensation = false };
            _dbContext.PersonProducts.AddRange(personProduct1, personProduct2, personProduct3);
            await _dbContext.SaveChangesAsync();

            var selectedPersonProduct = personProduct2;

            // Act
            await _service.UpdateCompensationFlags(productId, selectedPersonProduct);

            // Assert
            var updatedPersonProduct1 = await _dbContext.PersonProducts.FirstOrDefaultAsync(pp => pp.PersonId == person1Id && pp.ProductId == productId);
            var updatedPersonProduct2 = await _dbContext.PersonProducts.FirstOrDefaultAsync(pp => pp.PersonId == person2Id && pp.ProductId == productId);
            var updatedPersonProduct3 = await _dbContext.PersonProducts.FirstOrDefaultAsync(pp => pp.PersonId == person3Id && pp.ProductId == productId);

            Assert.NotNull(updatedPersonProduct1);
            Assert.NotNull(updatedPersonProduct2);
            Assert.NotNull(updatedPersonProduct3);

            Assert.Equal(false ,updatedPersonProduct1.Compensation);
            Assert.Equal(true ,updatedPersonProduct2.Compensation);
            Assert.Equal(false ,updatedPersonProduct3.Compensation);
        }

        [Fact]
        public async Task UpdatePartPricesPersonProduct_ShouldUpdatePersonProductPrices_WhenValidProductIsPassed()
        {
            // Arrange
            var product = new Product { Id = 1, Price = 100m, TotalPrice = 100m, MaxQuantity = 10 };

            var person1 = new Person { Id = 1, UserId = 1, Name = "Person 1", Surname = string.Empty };
            var person2 = new Person { Id = 2, UserId = 2, Name = "Person 2", Surname = string.Empty };
            _dbContext.Persons.AddRange(person1, person2);

            var personProduct1 = new PersonProduct { PersonId = person1.Id, ProductId = product.Id, Quantity = 5, PartOfPrice = 0 };
            var personProduct2 = new PersonProduct { PersonId = person2.Id, ProductId = product.Id, Quantity = 8, PartOfPrice = 0 };
            _dbContext.PersonProducts.AddRange(personProduct1, personProduct2);
            await _dbContext.SaveChangesAsync();

            // Act
            await _service.UpdatePartPricesPersonProduct(product);

            // Assert
            var updatedPersonProduct1 = await _dbContext.PersonProducts.FirstOrDefaultAsync(pp => pp.PersonId == person1.Id && pp.ProductId == product.Id);
            var updatedPersonProduct2 = await _dbContext.PersonProducts.FirstOrDefaultAsync(pp => pp.PersonId == person2.Id && pp.ProductId == product.Id);

            Assert.NotNull(updatedPersonProduct1);
            Assert.NotNull(updatedPersonProduct2);
            Assert.Equal(50m, updatedPersonProduct1.PartOfPrice);
            Assert.Equal(80m, updatedPersonProduct2.PartOfPrice);
        }

        [Fact]
        public async Task UpdateTotalPriceReceipt_ShouldUpdateTotalPriceCorrectly()
        {
            // Arrange
            var receipt = new Receipt(){ Id = 1, Name = "TestReceipt", TotalPrice = 0.0m };
            _dbContext.Receipts.Add(receipt);

            var product1 = new Product() { Id = 1, Name = "TestProduct1", Price = 15.99m, TotalPrice = 15.99m, ReceiptId = receipt.Id};
            var product2 = new Product() { Id = 2, Name = "TestProduct2", Price = 5.21m, TotalPrice = 5.21m, ReceiptId = receipt.Id };
            var product3 = new Product() { Id = 3, Name = "TestProduct3", Price = 8.55m, TotalPrice = 8.55m, ReceiptId = receipt.Id };
            _dbContext.Products.AddRange(product1, product2, product3);
            await _dbContext.SaveChangesAsync();

            // Act
            await _service.UpdateTotalPriceReceipt(receipt);

            // Assert
            var updatedReceipt = await _dbContext.Receipts.FirstOrDefaultAsync(pp => pp.Id == receipt.Id);
            Assert.NotNull(updatedReceipt);

            var totalPrice = product1.Price + product2.Price + product3.Price;
            Assert.Equal(totalPrice, updatedReceipt.TotalPrice);
        }

        [Fact]
        public async Task UpdateProductDetails_ShouldUpdateProductCorrectly()
        {
            // Arrange

            var product = new Product() { Id = 1, Name = "TestProduct1", Price = 10.00m, TotalPrice = 10.00m, Divisible = true, MaxQuantity = 10 };
            _dbContext.Products.Add(product);

            var personProduct = new PersonProduct { PersonId = 1, ProductId = product.Id, Quantity = 2, PartOfPrice = 20.00m};
            _dbContext.PersonProducts.Add(personProduct);
            await _dbContext.SaveChangesAsync();

            // Act
            await _service.UpdateProductDetails(product);

            // Assert
            var updatedProduct = await _dbContext.Products.FirstOrDefaultAsync(pp => pp.Id == product.Id);
            Assert.NotNull(updatedProduct);

            var availableQuantity = product.MaxQuantity - personProduct.Quantity;
            var compensationPrice = product.Price - personProduct.PartOfPrice;
            Assert.Equal(availableQuantity, updatedProduct.AvailableQuantity);
            Assert.Equal(compensationPrice, updatedProduct.CompensationPrice);
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
            decimal price = 100.00m;
            int purchasedQuantity = 2;
            decimal additionalPrice = 2.50m;
            int discountPercentage = 25;


            // Act
            decimal basePrice = price * purchasedQuantity;
            decimal discount = basePrice * discountPercentage / 100;
            decimal priceAfterDiscount = basePrice - discount;
            decimal expectedTotalPrice = priceAfterDiscount + additionalPrice;
            var result = _service.CalculateTotalPrice(price, purchasedQuantity, additionalPrice, discountPercentage);

            // Assert
            Assert.Equal(expectedTotalPrice, result);
        }
    }
}