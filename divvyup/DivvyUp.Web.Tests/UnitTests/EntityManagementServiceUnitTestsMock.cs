using Moq;
using DivvyUp_Shared.Models;
using DivvyUp.Web.Data;
using DivvyUp.Web.EntityManager;
using DivvyUp_Shared.Interfaces;
using DivvyUp_Shared.Exceptions;
using System.Net;

namespace DivvyUp.Web.Tests.UnitTests
{
    public class EntityManagementServiceUnitTestsMock
    {
        private readonly Mock<IDivvyUpDBContext> _dbContextMock;
        private readonly EntityManagementService _service;

        public EntityManagementServiceUnitTestsMock()
        {
            _dbContextMock = new Mock<IDivvyUpDBContext>();
            var userContextMock = new Mock<UserContext>(null, null);
            _service = new EntityManagementService(_dbContextMock.Object, userContextMock.Object);
        }

        [Fact]
        public async Task GetReceipt_ShouldReturnReceipt_WhenValidUserAndReceiptId()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "TestUser",
                Name = "TestUserName",
            };

            var receipt = new Receipt
            {
                Id = 1,
                UserId = user.Id,
                Name = "Test Receipt",
                TotalPrice = 100.00m
            };

            var users = new List<User> { user };
            var receipts = new List<Receipt> { receipt };

            var usersMock = users.ToMockDbSet();
            var receiptsMock = receipts.ToMockDbSet();

            _dbContextMock.Setup(db => db.Users).Returns(usersMock.Object);
            _dbContextMock.Setup(db => db.Receipts).Returns(receiptsMock.Object);

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
            var user = new User
            {
                Id = 1
            };

            var users = new List<User> { user };
            var receipts = new List<Receipt>();

            var usersMock = users.ToMockDbSet();
            var receiptsMock = receipts.ToMockDbSet();

            _dbContextMock.Setup(db => db.Users).Returns(usersMock.Object);
            _dbContextMock.Setup(db => db.Receipts).Returns(receiptsMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DException>(() => _service.GetReceipt(user, 999));

            Assert.Equal(HttpStatusCode.NotFound, exception.Status);
            Assert.Equal("Rachunek nie znaleziony", exception.Message);
        }

        [Fact]
        public async Task GetReceipt_ShouldThrowUnauthorizedException_WhenUserDoesNotOwnReceipt()
        {
            // Arrange
            var user1 = new User { Id = 1 };
            var user2 = new User { Id = 2 };

            var receipt = new Receipt { Id = 1, UserId = user1.Id, TotalPrice = 100.00m };

            var users = new List<User> { user1, user2 };
            var receipts = new List<Receipt> { receipt };

            var usersMock = users.ToMockDbSet();
            var receiptsMock = receipts.ToMockDbSet();

            _dbContextMock.Setup(db => db.Users).Returns(usersMock.Object);
            _dbContextMock.Setup(db => db.Receipts).Returns(receiptsMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DException>(() => _service.GetReceipt(user2, receipt.Id));

            Assert.Equal(HttpStatusCode.Unauthorized, exception.Status);
            Assert.Equal($"Brak dostępu do rachunku: {receipt.Id}", exception.Message);
        }

        [Fact]
        public async Task GetPersonWithLowestCompensation_ShouldReturnPersonWithLowestCompensation()
        {
            // Arrange
            int productId = 1;

            var persons = new List<Person>
            {
                new Person { Id = 1, CompensationAmount = 10.00m },
                new Person { Id = 2, CompensationAmount = 5.00m },
                new Person { Id = 3, CompensationAmount = 7.00m }
            };

            var personProducts = new List<PersonProduct>
            {
                new PersonProduct { Id = 1, PersonId = 1, ProductId = productId, Person = persons[0] },
                new PersonProduct { Id = 2, PersonId = 2, ProductId = productId, Person = persons[1] },
                new PersonProduct { Id = 3, PersonId = 3, ProductId = productId, Person = persons[2] }
            };

            _dbContextMock.Setup(db => db.Persons).Returns(persons.ToMockDbSet().Object);
            _dbContextMock.Setup(db => db.PersonProducts).Returns(personProducts.ToMockDbSet().Object);

            // Act
            var result = await _service.GetPersonWithLowestCompensation(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.PersonId);
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(false, true, true, false)]
        [InlineData(false, false, false, false)]
        public async Task AreAllPersonProductsSettled_ShouldReturnExpectedResult_WhenDifferentProductStatusesAreUsed(bool settled1, bool settled2, bool settled3, bool expected)
        {
            // Arrange
            int productId = 2;

            var personProducts = new List<PersonProduct>
            {
                new PersonProduct { Id = 1, ProductId = productId, Settled = settled1 },
                new PersonProduct { Id = 2, ProductId = productId, Settled = settled2 },
                new PersonProduct { Id = 3, ProductId = productId, Settled = settled3 }
            };

            _dbContextMock.Setup(db => db.PersonProducts).Returns(personProducts.ToMockDbSet().Object);

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

            var personProducts = new List<PersonProduct>
            {
                new PersonProduct { PersonId = person1Id, ProductId = productId, Compensation = true },
                new PersonProduct { PersonId = person2Id, ProductId = productId, Compensation = false },
                new PersonProduct { PersonId = person3Id, ProductId = productId, Compensation = false }
            };

            var personProductsMock = personProducts.ToMockDbSet();

            _dbContextMock.Setup(db => db.PersonProducts).Returns(personProductsMock.Object);

            _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var selectedPersonProduct = personProducts.First(pp => pp.PersonId == person2Id);

            // Act
            await _service.UpdateCompensationFlags(productId, selectedPersonProduct);

            // Assert
            _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            var updatedPersonProduct1 = personProducts.First(pp => pp.PersonId == person1Id && pp.ProductId == productId);
            var updatedPersonProduct2 = personProducts.First(pp => pp.PersonId == person2Id && pp.ProductId == productId);
            var updatedPersonProduct3 = personProducts.First(pp => pp.PersonId == person3Id && pp.ProductId == productId);

            Assert.NotNull(updatedPersonProduct1);
            Assert.NotNull(updatedPersonProduct2);
            Assert.NotNull(updatedPersonProduct3);

            Assert.False(updatedPersonProduct1.Compensation);
            Assert.True(updatedPersonProduct2.Compensation);
            Assert.False(updatedPersonProduct3.Compensation);
        }

        [Fact]
        public async Task UpdatePartPricesPersonProduct_ShouldUpdatePersonProductPrices_WhenValidProductIsPassed()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Price = 100m,
                TotalPrice = 100m,
                MaxQuantity = 10
            };

            var person1 = new Person
            {
                Id = 1,
                UserId = 1,
            };

            var person2 = new Person
            {
                Id = 2,
                UserId = 2,
            };

            var persons = new List<Person> { person1, person2 };
            var personProducts = new List<PersonProduct>
            {
                new PersonProduct { PersonId = person1.Id, ProductId = product.Id, Quantity = 5, PartOfPrice = 0 },
                new PersonProduct { PersonId = person2.Id, ProductId = product.Id, Quantity = 8, PartOfPrice = 0 }
            };

            var personsMock = persons.ToMockDbSet();
            var personProductsMock = personProducts.ToMockDbSet();
            var productsMock = new List<Product> { product }.ToMockDbSet();

            _dbContextMock.Setup(db => db.Persons).Returns(personsMock.Object);
            _dbContextMock.Setup(db => db.PersonProducts).Returns(personProductsMock.Object);
            _dbContextMock.Setup(db => db.Products).Returns(productsMock.Object);

            _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(1);

            // Act
            await _service.UpdatePartPricesPersonProduct(product);

            // Assert
            _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            var updatedPersonProduct1 = personProducts.FirstOrDefault(pp => pp.PersonId == person1.Id && pp.ProductId == product.Id);
            var updatedPersonProduct2 = personProducts.FirstOrDefault(pp => pp.PersonId == person2.Id && pp.ProductId == product.Id);

            Assert.NotNull(updatedPersonProduct1);
            Assert.NotNull(updatedPersonProduct2);

            Assert.Equal(50m, updatedPersonProduct1.PartOfPrice);
            Assert.Equal(80m, updatedPersonProduct2.PartOfPrice);
        }

        [Fact]
        public async Task UpdateProductDetails_ShouldUpdateProductCorrectly()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Price = 10.00m,
                TotalPrice = 10.00m,
                Divisible = true,
                MaxQuantity = 10,
                AvailableQuantity = 10,
                CompensationPrice = 0.00m
            };

            var personProduct = new PersonProduct
            {
                PersonId = 1,
                ProductId = product.Id,
                Quantity = 2,
                PartOfPrice = 20.00m
            };

            var products = new List<Product> { product };
            var personProducts = new List<PersonProduct> { personProduct };

            var productsMock = products.ToMockDbSet();
            var personProductsMock = personProducts.ToMockDbSet();

            _dbContextMock.Setup(db => db.Products).Returns(productsMock.Object);
            _dbContextMock.Setup(db => db.PersonProducts).Returns(personProductsMock.Object);

            // Act
            await _service.UpdateProductDetails(product);

            // Assert
            var updatedProduct = products.FirstOrDefault(p => p.Id == product.Id);
            Assert.NotNull(updatedProduct);

            var expectedAvailableQuantity = product.MaxQuantity - personProduct.Quantity;
            var expectedCompensationPrice = product.Price - personProduct.PartOfPrice;

            Assert.Equal(expectedAvailableQuantity, updatedProduct.AvailableQuantity);
            Assert.Equal(expectedCompensationPrice, updatedProduct.CompensationPrice);
        }
    }
}