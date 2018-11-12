using System;
using PCT.Shop.Core.Domain;
using Xunit;

namespace PCT.Shop.Core.UnitTests
{

    public class CartItemTests
    {
        [Fact]
        public void Given_Product_When_Valid_Then_CreateCartItemWithDefaultQuantity()
        {
            // Arrange
            var product = new Product("Milk", 1.0m);
            
            // Act
            var result = new CartItem(product, 1);
            
            // Assert
            Assert.Equal("Milk", result.Name);
            Assert.Equal(1.0m, result.UnitPrice);
            Assert.Equal(1, result.Quantity);
        }

        [Theory]
        [InlineData(2, 3.0)]
        [InlineData(3, 4.5)]
        [InlineData(4, 6.0)]
        public void Given_Product_When_ItemTotalRequested_Then_ReturnTotalForQuantity(int quantity, decimal totalPrice)
        {
            // Arrange
            var product = new Product("Bread", 1.5m);
            
            // Act
            var result = new CartItem(product, quantity);
            
            // Assert
            Assert.Equal(totalPrice, result.TotalPrice);
        }

        [Fact]
        public void Given_NoProduct_When_CartItemBuilt_Then_Throw()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new CartItem(null, 1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Given_Product_When_QuantityIsZeroOrLess_Then_Throw(int quantity)
        {
            // Arrange
            var product = new Product("Bread", 1.5m);

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new CartItem(product, quantity));
        }

        [Fact]
        public void Given_CartItem_When_QuantityIncreased_Then_ReturnNewItemWithUpdatedQuantity()
        {
            // Arrange
            var product = new Product("Milk", 1.0m);
            var cartItem = new CartItem(product, 1);

            // Act
            var result = cartItem.IncreaseQuantity(5);

            // Assert
            Assert.NotSame(cartItem, result);
            Assert.Equal(6, result.Quantity);
        }

        [Fact]
        public void Given_CartItem_When_QuantityDecreased_Then_ReturnNewItemWithUpdatedQuantity()
        {
            // Arrange
            var product = new Product("Milk", 1.0m);
            var cartItem = new CartItem(product, 5);

            // Act
            var result = cartItem.DecreaseQuantity(3);

            // Assert
            Assert.NotSame(cartItem, result);
            Assert.Equal(2, result.Quantity);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(600)]
        public void Given_CartItem_When_QuantityDecreasedByMoreThanPossible_Then_AlwaysReturnItemWithQuantityOfOne(int decreaseQuantityBy)
        {
            // Arrange
            var product = new Product("Milk", 1.0m);
            var cartItem = new CartItem(product, 5);

            // Act
            var result = cartItem.DecreaseQuantity(decreaseQuantityBy);

            // Assert
            Assert.NotSame(cartItem, result);
            Assert.Equal(1, result.Quantity);
        }
    }
}
