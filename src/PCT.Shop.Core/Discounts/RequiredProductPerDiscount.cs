using System;

namespace PCT.Shop.Core.Discounts
{
    public sealed class RequiredProductPerDiscount
    {
        public string ProductName { get; }
        public int Quantity { get; }

        public RequiredProductPerDiscount(string productName, int quantity)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(productName));
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));

            ProductName = productName;
            Quantity = quantity;
        }
    }
}