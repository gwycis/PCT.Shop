using System;

namespace PCT.Shop.Core.Domain
{
    public class DiscountDescriptor
    {
        public decimal Amount { get; }
        public string Description { get; }
        
        public DiscountDescriptor(decimal amount, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(description));
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
            Description = description;
            Amount = amount;
        }
    }
}
