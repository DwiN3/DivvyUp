using DivvyUp.Web.Validation;
using DivvyUp_Shared.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DivvyUp.Web.Tests.UnitTests
{
    public class DValidatorUnitTests
    {
        private readonly DValidator _validator;

        public DValidatorUnitTests()
        {
            _validator = new DValidator();
        }

        [Fact]
        public void IsNull_WhenObjectIsNull_ShouldThrowException()
        {
            // Arrange
            object obj = null;

            // Act && Assert
            var exception = Assert.Throws<DException>(() => _validator.IsNull(obj, "Object is null"));
            Assert.Equal(HttpStatusCode.BadRequest, exception.Status);
            Assert.Equal("Object is null", exception.Message);
        }

        [Fact]
        public void IsEmpty_WhenStringIsEmpty_ShouldThrowException()
        {
            // Arrange
            var str = string.Empty;

            // Act && Assert
            var exception = Assert.Throws<DException>(() => _validator.IsEmpty(str, "String is empty"));
            Assert.Equal(HttpStatusCode.BadRequest, exception.Status);
            Assert.Equal("String is empty", exception.Message);
        }

        [Fact]
        public void IsMinusValue_WhenValueIsMinus_ShouldThrowException()
        {
            // Arrange
            var value = -10.00m;

            // Act && Assert
            var exception = Assert.Throws<DException>(() => _validator.IsMinusValue(value, "Value is minus"));
            Assert.Equal(HttpStatusCode.BadRequest, exception.Status);
            Assert.Equal("Value is minus", exception.Message);
        }

        [Fact]
        public void IsCorrectDataRange_WhenValueIsMinus_ShouldThrowException()
        {
            // Arrange
            DateOnly dateFrom = new DateOnly(2024, 12, 24);
            DateOnly dateTo = new DateOnly(2024, 12, 7);

            // Act && Assert
            var exception = Assert.Throws<DException>(() => _validator.IsCorrectDataRange(dateFrom, dateTo));
            Assert.Equal(HttpStatusCode.BadRequest, exception.Status);
            Assert.Equal("Zakres dat jest źle ustawiony", exception.Message);
        }

        [Fact]
        public void IsCorrectPercentageRange_WhenValueIsOutOfRange_ShouldThrowException()
        {
            // Arrange
            var value = -5;

            // Act
            var exception = Assert.Throws<DException>(() =>
            {
                _validator.IsCorrectPercentageRange(value);
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, exception.Status);
            Assert.Equal("Wartość procentowa jest błędnie ustawiona", exception.Message);
        }
    }
}
