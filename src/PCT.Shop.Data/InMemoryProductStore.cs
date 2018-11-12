using System;
using System.Collections.Generic;
using PCT.Shop.Core.Data;
using PCT.Shop.Core.Domain;

namespace PCT.Shop.Data
{
    public sealed class InMemoryProductStore : IProductStore
    {
        private readonly List<Product> _products;

        public InMemoryProductStore()
        {
            _products = new List<Product>
            {
                new Product("Soup", 0.65m),
                new Product("Milk", 1.30m),
                new Product("Bread", 0.80m),
                new Product("Apples", 1.00m),
                new Product("Oranges", 1.50m),
                new Product("Bananas", 1.20m),
                new Product("Cheese", 2.20m)
            };
        }

        public IReadOnlyList<Product> GetProducts()
        {
            return _products;
        }
    }
}
