using System;
using System.Collections.Generic;
using PCT.Shop.Core.Discounts;
using PCT.Shop.Core.Domain;
using Xunit;

namespace PCT.Shop.Core.UnitTests
{
    public class FixedValueDiscountTests
    {
        [Theory]
        [InlineData("Oranges", 1, 0.50, 0.50, "Oranges 0.5£ off: -50p")]
        [InlineData("Oranges", 5, 0.50, 2.50, "Oranges 0.5£ off: -£2.50")]
        [InlineData("Pears", 1, 0.50, 0.50, "Pears 0.5£ off: -50p")]
        [InlineData("Pears", 5, 0.50, 2.50, "Pears 0.5£ off: -£2.50")]
        [InlineData("Plums", 1, 0.50, 0.50, "Plums 0.5£ off: -50p")]
        [InlineData("Plums", 5, 0.50, 2.50, "Plums 0.5£ off: -£2.50")]
        [InlineData("Oranges", 1, 0.75, 0.75, "Oranges 0.75£ off: -75p")]
        [InlineData("Oranges", 5, 0.75, 3.75, "Oranges 0.75£ off: -£3.75")]
        [InlineData("Pears", 1, 0.75, 0.75, "Pears 0.75£ off: -75p")]
        [InlineData("Pears", 5, 0.75, 3.75, "Pears 0.75£ off: -£3.75")]
        [InlineData("Plums", 1, 0.75, 0.75, "Plums 0.75£ off: -75p")]
        [InlineData("Plums", 5, 0.75, 3.75, "Plums 0.75£ off: -£3.75")]
        public void Given_Cart_When_DiscountedProductFound_Then_ApplyDiscount(
            string product, 
            int quantity, 
            decimal discountAmount,
            decimal expectedDiscount,
            string expectedDescription)
        {
            // Arrange
            var items = new List<CartItem>
            {
                new CartItem(new Product("Milk", 1.0m), 1),
                new CartItem(new Product("Tomato", 0.50m), 1),
                new CartItem(new Product(product, 1.5m), quantity)
            };

            var sut = new FixedValueDiscount(product, discountAmount);

            // Act
            var result = sut.Apply(items);

            // Assert
            Assert.Collection(result, d =>
            {
                Assert.Equal(expectedDescription, d.Description);
                Assert.Equal(expectedDiscount, d.Amount);
            });
        }
    }
}