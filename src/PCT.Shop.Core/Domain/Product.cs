using System;

namespace PCT.Shop.Core.Domain
{
    public sealed class Product 
    {
        public string Name { get; }
        public decimal Price { get; }

        public Product(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price));

            Name = name;
            Price = price;
        }
    }
}