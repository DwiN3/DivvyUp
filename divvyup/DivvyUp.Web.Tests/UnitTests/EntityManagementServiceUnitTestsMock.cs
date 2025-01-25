using Moq;
using DivvyUp_Shared.Models;
using DivvyUp.Web.Data;
using DivvyUp.Web.EntityManager;
using DivvyUp_Shared.Interfaces;

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
    }
}
