using System;
using System.Collections.Generic;

namespace PCT.Shop.Core.Domain
{

    public abstract class Discount : IDiscount
    {
        public string ProductName { get; }

        protected Discount(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(productName));
            ProductName = productName;
        }

        protected abstract bool CanApply(IEnumerable<CartItem> items);
        protected abstract DiscountDescriptor CalculateDiscount(IEnumerable<CartItem> items);

        public IEnumerable<DiscountDescriptor> Apply(IEnumerable<CartItem> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            var discounts = new List<DiscountDescriptor>();

            if (CanApply(items))
            {
                discounts.Add(CalculateDiscount(items));
            }

            return discounts;
        }

        protected string FormatAmount(decimal discountAmount)
        {
            if (discountAmount >= 1.0m)
            {
                return $"{discountAmount:C2}";
            }

            return $"{discountAmount * 100:##}p";
        }

    }
}