using System;
using PCT.Shop.Core.Domain;
using Xunit;

namespace PCT.Shop.Core.UnitTests
{
    public class ProductTests
    {
        [Fact]
        public void Given_ProductDetails_When_DetailsAreValid_Then_CreateProduct()
        {
            // Arrange
            // Act
            var result = new Product("Milk", 1.0m);

            // Assert
            Assert.Equal("Milk", result.Name);
            Assert.Equal(1.0m, result.Price);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("     ")]
        public void Given_ProductDetails_When_NameIsNotValid_Then_Throw(string name)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Product(name, 1.0m));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Given_ProductDetails_When_PriceIsNotValid_Then_Throw(decimal price)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Product("Milk", price));
        }
    }
}