using System;
using System.Collections.Generic;
using System.Linq;
using PCT.Shop.Core.Domain;

namespace PCT.Shop.Core.Discounts
{
    public sealed class PercentageDiscount : Discount
    {
        private readonly DateTime _expiresAt;
        
        private decimal DiscountInPercent { get; }

        DateTime ExpiresAt => DateTime.Today;


        public PercentageDiscount(string productName, decimal discountInPercent, DateTime expiresAt) : base(productName)
        {
            if (discountInPercent <= 0) throw new ArgumentOutOfRangeException(nameof(discountInPercent));
            _expiresAt = expiresAt;
            DiscountInPercent = discountInPercent;
        }
        protected override bool CanApply(IEnumerable<CartItem> items)
        {
            return items.Any(i => i.Name.Equals(ProductName) && i.Name.Equals(ExpiresAt.Equals(false));
        }

        protected override DiscountDescriptor CalculateDiscount(IEnumerable<CartItem> items)
        {
            var itemToDiscount = items.Single(i => i.Name.Equals(ProductName));
            var discountAmount = itemToDiscount.TotalPrice * DiscountInPercent / 100;
            var formattedAmount = FormatAmount(discountAmount);
            var description = $"{ProductName} {DiscountInPercent}% off: -{formattedAmount}";

            return  new DiscountDescriptor(discountAmount, description);
        }
    }
}