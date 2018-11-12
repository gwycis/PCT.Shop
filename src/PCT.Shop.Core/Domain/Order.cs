using System;
using System.Collections.Generic;

namespace PCT.Shop.Core.Domain
{
    public sealed class Order
    {
        public decimal SubTotal { get; }
        public decimal Total { get; }
        public IEnumerable<DiscountDescriptor> Discounts { get; }

        public Order(decimal subTotal, decimal total, IEnumerable<DiscountDescriptor> discounts)
        {
            if (subTotal < 0) throw new ArgumentOutOfRangeException(nameof(subTotal));
            if (total < 0) throw new ArgumentOutOfRangeException(nameof(total));

            SubTotal = subTotal;
            Total = total;
            Discounts = discounts;
        }
    }
}