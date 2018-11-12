using System;
using System.Collections.Generic;
using NSubstitute;
using PCT.Shop.Core.Data;
using PCT.Shop.Core.Domain;
using Xunit;

namespace PCT.Shop.Core.UnitTests
{
    public class CartTests
    {
        private Product Milk { get; }
        private Product Bread { get; }
        private Product Apples { get; }
        private Cart Sut { get; }
        private IDiscountStore DiscountStore { get; }

        public CartTests()
        {
            Milk = new Product("Milk", 1.0m);
            Bread = new Product("Bread", 1.5m);
            Apples = new Product("Apples", 1.25m);

            DiscountStore = Substitute.For<IDiscountStore>();
            Sut = new Cart(DiscountStore);

            DiscountStore.GetDiscounts().Returns(new List<Discount>());
        }

        [Fact]
        public void
            Given_EmptyCart_When_MultipleProductsAdded_And_MilkAddedTwice_Then_CartShouldContainAllProductsWithCorrectQuantities()
        {
            // Arrange
            // Act
            Sut.Add(Milk);
            Sut.Add(Bread);
            Sut.Add(Milk);

            // Assert
            Assert.Collection(Sut.Items, i =>
            {
                Assert.Equal("Milk", i.Name);
                Assert.Equal(2, i.Quantity);
            }, i =>
            {
                Assert.Equal("Bread", i.Name);
                Assert.Equal(1, i.Quantity);
            });
        }

        [Fact]
        public void Given_EmptyCart_When_AddingProduct_And_SuppliedValueIsNull_Then_Throw()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => Sut.Add(null));
        }

        [Fact]
        public void Given_Cart_When_RemovingProduct_And_SuppliedValueIsNull_Then_Throw()
        {
            // Arrange
            Sut.Add(Milk);
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => Sut.Remove(null));
        }

        [Fact]
        public void Given_EmptyCart_When_MultipleProductsAdded_Then_CartShouldContainAllProducts()
        {
            // Arrange
            // Act
            Sut.Add(Milk);
            Sut.Add(Bread);

            // Assert
            Assert.Collection(Sut.Items, i =>
            {
                Assert.Equal("Milk", i.Name);
                Assert.Equal(1, i.Quantity);
            }, i =>
            {
                Assert.Equal("Bread", i.Name);
                Assert.Equal(1, i.Quantity);
            });
        }

        [Fact]
        public void Given_EmptyCart_When_ProductAdded_Then_CartShouldContainProduct()
        {
            // Arrange
            // Act
            Sut.Add(Milk);

            // Assert
            Assert.Collection(Sut.Items, i =>
            {
                Assert.Equal("Milk", i.Name);
                Assert.Equal(1, i.Quantity);
            });
        }

        [Fact]
        public void Given_Cart_When_ProductRemoved_And_ProductQuantityIsOne_Then_RemoveProductFromCart()
        {
            // Arrange
            // Act
            Sut.Add(Milk);
            Sut.Add(Bread);
            Sut.Remove(Milk);

            // Assert
            Assert.Collection(Sut.Items, i =>
            {
                Assert.Equal("Bread", i.Name);
                Assert.Equal(1, i.Quantity);
            });
        }

        [Fact]
        public void Given_Cart_When_ProductRemoved_And_ProductQuantityIsMoreThanOne_Then_DecreaseProductQuantity()
        {
            // Arrange
            // Act
            Sut.Add(Milk);
            Sut.Add(Milk);
            Sut.Add(Milk);
            Sut.Add(Bread);
            Sut.Remove(Milk);

            // Assert
            Assert.Collection(Sut.Items, i =>
            {
                Assert.Equal("Milk", i.Name);
                Assert.Equal(2, i.Quantity);
            }, i =>
            {
                Assert.Equal("Bread", i.Name);
                Assert.Equal(1, i.Quantity);
            });
        }

        [Fact]
        public void Given_Cart_When_ProductRemovedMoreTimesThanQuantityInCart_Then_CartShouldNotHaveProduct()
        {
            // Arrange
            // Act
            Sut.Add(Milk);
            Sut.Add(Milk);
            Sut.Add(Bread);
            Sut.Remove(Milk);
            Sut.Remove(Milk);
            Sut.Remove(Milk);

            // Assert
            Assert.Collection(Sut.Items, i =>
            {
                Assert.Equal("Bread", i.Name);
                Assert.Equal(1, i.Quantity);
            });
        }

        [Fact]
        public void Given_Cart_When_ProductThatIsNotInCartIsRemoved_Then_CartShouldRemainUnchanged()
        {
            // Arrange
            // Act
            Sut.Add(Milk);
            Sut.Add(Bread);
            Sut.Remove(Apples);

            // Assert
            Assert.Collection(Sut.Items, i =>
            {
                Assert.Equal("Milk", i.Name);
                Assert.Equal(1, i.Quantity);
            }, i =>
            {
                Assert.Equal("Bread", i.Name);
                Assert.Equal(1, i.Quantity);
            });
        }

        [Fact]
        public void Given_Cart_When_HasNoProducts_Then_TotalShouldBeZero()
        {
            // Arrange
            // Act
            var result = Sut.Checkout();

            // Assert
            Assert.Equal(0.0m, result.SubTotal);
            Assert.Equal(0.0m, result.Total);
        }

        [Fact]
        public void Given_Cart_When_NoDiscountsAvailable_Then_ReturnTotal()
        {
            // Arrange
            // Act
            Sut.Add(Milk);
            Sut.Add(Bread);

            var result = Sut.Checkout();

            // Assert
            Assert.Equal(2.5m, result.SubTotal);
            Assert.Equal(2.5m, result.Total);
            Assert.Empty(result.Discounts);
        }

        [Fact]
        public void Given_Cart_When_DiscountsAvailable_ButNotApplicable_Then_ReturnTotal()
        {
            // Arrange
            var discount = Substitute.For<IDiscount>();
            DiscountStore.GetDiscounts().Returns(new List<IDiscount>
            {
                discount
            });

            discount.Apply(Arg.Any<IEnumerable<CartItem>>()).Returns(new List<DiscountDescriptor>());

            // Act
            Sut.Add(Milk);
            Sut.Add(Bread);

            var result = Sut.Checkout();

            // Assert
            Assert.Equal(2.5m, result.SubTotal);
            Assert.Equal(2.5m, result.Total);
            Assert.Empty(result.Discounts);
        }

        [Fact]
        public void Given_Cart_When_DiscountsApplicable_Then_ReturnDiscountedTotal()
        {
            // Arrange
            var applicableDiscount = Substitute.For<IDiscount>();
            var nonApplicableDiscount = Substitute.For<IDiscount>();

            DiscountStore.GetDiscounts().Returns(new List<IDiscount>
            {
                applicableDiscount,
                nonApplicableDiscount
            });

            nonApplicableDiscount.Apply(Arg.Any<IEnumerable<CartItem>>()).Returns(new List<DiscountDescriptor>());
            applicableDiscount.Apply(Arg.Any<IEnumerable<CartItem>>()).Returns(new List<DiscountDescriptor>
            {
                new DiscountDescriptor(0.5m, "Milk 50% off!")
            });

            // Act
            Sut.Add(Milk);
            Sut.Add(Bread);

            var result = Sut.Checkout();

            // Assert
            Assert.Equal(2.5m, result.SubTotal);
            Assert.Equal(2.0m, result.Total);
            Assert.Collection(result.Discounts, d =>
            {
                Assert.Equal(0.5m, d.Amount);
                Assert.Equal("Milk 50% off!", d.Description);
            });
        }
    }
}