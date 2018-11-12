using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCT.Shop.Core.Domain;

namespace PCT.Shop.Core.Discounts
{
   public sealed class FixedValueDiscount : Discount
    {
        public decimal DiscountInValue { get; }

        public FixedValueDiscount(string productName, decimal discountInValue) : base(productName)
        {
            if (discountInValue <= 0) throw new ArgumentOutOfRangeException(nameof(discountInValue));
            DiscountInValue = discountInValue;
        }

        protected override bool CanApply(IEnumerable<CartItem> items)
        {
            return items.Any(i => i.Name.Equals(ProductName));
        }

        protected override DiscountDescriptor CalculateDiscount(IEnumerable<CartItem> items)
        {
            var itemToDiscount = items.Single(i => i.Name.Equals(ProductName));
            var discountAmount = itemToDiscount.Quantity * DiscountInValue;
            var formattedAmount = FormatAmount(discountAmount);
            var description = $"{ProductName} {DiscountInValue}£ off: -{formattedAmount}";

            return new DiscountDescriptor(discountAmount, description);
        }
    }
}
