using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NSubstitute;
using PCT.Shop.Core.Discounts;
using PCT.Shop.Core.Domain;
using Xunit;
using Xunit.Abstractions;

namespace PCT.Shop.Core.UnitTests
{
    public class PercentageDiscountTests
    {
        [Fact]
        public void Given_Cart_When_NoDiscountedProductFound_Then_NoDiscountApplies()
        {
            // Arrange
            var items = new List<CartItem>
            {
                new CartItem(new Product("Milk", 1.0m), 1),
                new CartItem(new Product("Tomato", 0.50m), 1)
            };

            var sut = new PercentageDiscount("Apples", 10, new DateTime(2018,11,19));

            // Act
            var result = sut.Apply(items);

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(1, 0.10, "Apples 10% off: -10p")]
        [InlineData(5, 0.50, "Apples 10% off: -50p")]
        [InlineData(11, 1.10, "Apples 10% off: -£1.10")]
        public void Given_Cart_When_DiscountedProductFound_Then_ApplyDiscount(int quantity, decimal expectedDiscount, string expectedDescription)
        {
            // Arrange
            var items = new List<CartItem>
            {
                new CartItem(new Product("Milk", 1.0m), 1),
                new CartItem(new Product("Tomato", 0.50m), 1),
                new CartItem(new Product("Apples", 1.0m), quantity)
            };

            var sut = new PercentageDiscount("Apples", 10, new DateTime(2018,11,19));

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
        public void Given_Cart_When_DiscountExpires_Then_RemoveDiscount()
        {

            var expires = Substitute.For<IDateTime>(new DateTime(2018,11,19));
            var items = new List<CartItem>
            {
                new CartItem(new Product("Milk", 1.0m), 1),
                new CartItem(new Product("Tomato", 0.50m), 1),
                new CartItem(new Product("Apples", 1.0m), 1)
                
            };

            var sut = new PercentageDiscount("Apples", 10, new DateTime(2018,11,19));

            // Act
            var result = sut.Apply(items);

            // Assert
            Assert.Equal(expires, result);
            
        }
    }
}