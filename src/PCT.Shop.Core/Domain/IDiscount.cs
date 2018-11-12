using System.Collections.Generic;

namespace PCT.Shop.Core.Domain
{
    public interface IDiscount
    {
        string ProductName { get; }
        IEnumerable<DiscountDescriptor> Apply(IEnumerable<CartItem> items);
    }
}