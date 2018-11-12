using System;
using System.Collections.Generic;
using PCT.Shop.Core.Discounts;
using PCT.Shop.Core.Domain;
using Xunit;

namespace PCT.Shop.Core.UnitTests
{
    public class MultiBuyPercentageDiscountTests
    {
        [Theory]
        [InlineData(1, 2, 0.40, "Bread 50% off: -40p")]
        [InlineData(2, 2, 0.40, "Bread 50% off: -40p")]
        [InlineData(2, 3, 0.40, "Bread 50% off: -40p")]
        [InlineData(2, 4, 0.80, "Bread 50% off: -80p")]
        [InlineData(2, 5, 0.80, "Bread 50% off: -80p")]
        [InlineData(3, 4, 0.80, "Bread 50% off: -80p")]
        [InlineData(3, 5, 0.80, "Bread 50% off: -80p")]
        [InlineData(3, 6, 1.20, "Bread 50% off: -£1.20")]
        public void Given_Cart_When_DiscountedProductFoundBread_And_AssociatedProductsFound_Then_ApplyCorrectDiscount(int breadCount, int soupCount, decimal expectedDiscount, string expectedDescription)
        {
            // Arrange
            var prerequisite = new RequiredProductPerDiscount("Soup", 2);

            var items = new List<CartItem>
            {
                new CartItem(new Product("Milk", 1.0m), 1),
                new CartItem(new Product("Tomato", 0.50m), 1),
                new CartItem(new Product("Bread", 0.80m), breadCount),
                new CartItem(new Product("Soup", 0.65m), soupCount)
            };

            var sut = new MultiBuyPercentageDiscount("Bread", 50, prerequisite);

            // Act
            var result = sut.Apply(items);

            // Assert
            Assert.Collection(result, d =>
            {
                Assert.Equal(expectedDescription, d.Description);
                Assert.Equal(expectedDiscount, d.Amount);
            });
        }

        [Theory]
        [InlineData(1, 10, 1.30, "Bananas 100% off: -£1.30")]
        [InlineData(2, 10, 1.30, "Bananas 100% off: -£1.30")]
        [InlineData(3, 20, 2.60, "Bananas 100% off: -£2.60")]
        [InlineData(3, 25, 2.60, "Bananas 100% off: -£2.60")]
        [InlineData(1, 100, 1.30, "Bananas 100% off: -£1.30")]
        [InlineData(10, 50, 6.50, "Bananas 100% off: -£6.50")]
        public void Given_Cart_When_DiscountedProductFoundBananas_And_AssociatedProductsFound_Then_ApplyCorrectDiscount(
            int bananasCount, int orangesCount, decimal expectedDiscount, string expectedDescription)
        {
            // Arrange
            var prerequisite = new RequiredProductPerDiscount("Oranges", 10);

            var items = new List<CartItem>
            {
                new CartItem(new Product("Milk", 1.0m), 1),
                new CartItem(new Product("Tomato", 0.50m), 1),
                new CartItem(new Product("Bananas", 1.30m), bananasCount),
                new CartItem(new Product("Oranges", 1.50m), orangesCount)
            };

            var sut = new MultiBuyPercentageDiscount("Bananas", 100, prerequisite);

            // Act
            var result = sut.Apply(items);

            // Assert
            Assert.Collection(result, d =>
            {
                Assert.Equal(expectedDescription, d.Description);
                Assert.Equal(expectedDiscount, d.Amount);
            });
        }

        [Fact]
        public void Given_Cart_When_DiscountedProductFound_But_AssociatedProductsNotFound_Then_NoDiscountApplies()
        {
            // Arrange
            var prerequisite = new RequiredProductPerDiscount("Soup", 2);

            var items = new List<CartItem>
            {
                new CartItem(new Product("Milk", 1.0m), 1),
                new CartItem(new Product("Tomato", 0.50m), 1),
                new CartItem(new Product("Bread", 0.80m), 1)
            };

            var sut = new MultiBuyPercentageDiscount("Bread", 50, prerequisite);

            // Act
            var result = sut.Apply(items);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Given_Cart_When_NoDiscountedProductFound_Then_NoDiscountApplies()
        {
            // Arrange
            var prerequisite = new RequiredProductPerDiscount("Soup", 2);

            var items = new List<CartItem>
            {
                new CartItem(new Product("Milk", 1.0m), 1),
                new CartItem(new Product("Tomato", 0.50m), 1)
            };

            var sut = new MultiBuyPercentageDiscount("Bread", 50, prerequisite);

            // Act
            var result = sut.Apply(items);

            // Assert
            Assert.Empty(result);
        }
    }
}