using DivvyUp.Web.Data;
using DivvyUp.Web.Validation;
using DivvyUp_Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DivvyUp.Web.Tests.UnitTests
{
    public class DValidatorUnitTests
    {
        private readonly DValidator _validator;

        public DValidatorUnitTests()
        {
            var options = new DbContextOptionsBuilder<DivvyUpDBContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            var dbContext = new DivvyUpDBContext(options);
            _validator = new DValidator(dbContext);
        }

        [Fact]
        public void IsNull_WhenObjectIsNull_ShouldThrowException()
        {
            // Arrange
            object obj = null;

            // Act
            var exception = Assert.Throws<DException>(() => _validator.IsNull(obj, "Object is null"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, exception.Status);
            Assert.Equal("Object is null", exception.Message);
        }

        [Fact]
        public void IsEmpty_WhenStringIsEmpty_ShouldThrowException()
        {
            // Arrange
            var str = string.Empty;

            // Act
            var exception = Assert.Throws<DException>(() => _validator.IsEmpty(str, "String is empty"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, exception.Status);
            Assert.Equal("String is empty", exception.Message);
        }

        [Fact]
        public void IsMinusValue_WhenValueIsMinus_ShouldThrowException()
        {
            // Arrange
            var value = -10.00m;

            // Act
            var exception = Assert.Throws<DException>(() => _validator.IsMinusValue(value, "Value is minus"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, exception.Status);
            Assert.Equal("Value is minus", exception.Message);
        }
    }
}
