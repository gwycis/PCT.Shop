using System;
using System.Collections.Generic;
using System.Linq;
using PCT.Shop.Core.Domain;

namespace PCT.Shop.Core.Discounts
{
    public sealed class MultiBuyPercentageDiscount : Discount
    {
        public MultiBuyPercentageDiscount(string productName, decimal discountInPercent,
            RequiredProductPerDiscount requiredProductPerDiscount) : base(productName)
        {
            if (discountInPercent <= 0) throw new ArgumentOutOfRangeException(nameof(discountInPercent));
            DiscountInPercent = discountInPercent;
            RequiredProductPerDiscount = requiredProductPerDiscount ??
                                         throw new ArgumentNullException(nameof(requiredProductPerDiscount));
        }

        public decimal DiscountInPercent { get; }
        public RequiredProductPerDiscount RequiredProductPerDiscount { get; }

        protected override bool CanApply(IEnumerable<CartItem> items)
        {
            return items.Any(i => i.Name.Equals(ProductName)) &&
                   items.Any(i => i.Name.Equals(RequiredProductPerDiscount.ProductName) && i.Quantity >= RequiredProductPerDiscount.Quantity);
        }

        protected override DiscountDescriptor CalculateDiscount(IEnumerable<CartItem> items)
        {
            var itemToDiscount = items.Single(i => i.Name.Equals(ProductName));
            var timesDiscountCanBeApplied = CalculateTimesToApplyDiscount(itemToDiscount, items);
            var discountAmount = itemToDiscount.UnitPrice * timesDiscountCanBeApplied * DiscountInPercent / 100;
            var formattedAmount = FormatAmount(discountAmount);
            var description = $"{ProductName} {DiscountInPercent}% off: -{formattedAmount}";

            return new DiscountDescriptor(discountAmount, description);
        }

        private int CalculateTimesToApplyDiscount(CartItem itemToDiscount, IEnumerable<CartItem> items)
        {
            var requiredItem = items.Single(i => i.Name.Equals(RequiredProductPerDiscount.ProductName));
            decimal result = requiredItem.Quantity / RequiredProductPerDiscount.Quantity;
            decimal howManyTimesCanDiscountBeApplied = Math.Round(result, MidpointRounding.ToEven);

            if (itemToDiscount.Quantity < howManyTimesCanDiscountBeApplied)
                return itemToDiscount.Quantity;

            return Convert.ToInt32(howManyTimesCanDiscountBeApplied);
        }
    }
}