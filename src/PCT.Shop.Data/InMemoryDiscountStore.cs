using System;
using System.Collections.Generic;
using PCT.Shop.Core.Data;
using PCT.Shop.Core.Discounts;
using PCT.Shop.Core.Domain;

namespace PCT.Shop.Data
{
    public sealed class InMemoryDiscountStore : IDiscountStore
    {
        private readonly List<IDiscount> _discounts;

        public InMemoryDiscountStore()
        {
            _discounts = new List<IDiscount>
            {
                new PercentageDiscount("Apples", 10.0m, new DateTime(2018,11,19)),
                new FixedValueDiscount("Oranges", 0.5m),
                new MultiBuyPercentageDiscount("Bread", 50.0m, new RequiredProductPerDiscount("Soup", 2)),
                new MultiBuyPercentageDiscount("Bananas", 100.0m, new RequiredProductPerDiscount("Oranges", 10))
            };
        }

        public IReadOnlyList<IDiscount> GetDiscounts()
        {
            return _discounts;
        }
    }
}