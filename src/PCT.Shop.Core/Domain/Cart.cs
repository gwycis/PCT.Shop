using System;
using System.Collections.Generic;
using System.Linq;
using PCT.Shop.Core.Data;

namespace PCT.Shop.Core.Domain
{
    public sealed class Cart
    {
        private readonly IDiscountStore _discountStore;
        private readonly Dictionary<string, CartItem> _items = new Dictionary<string, CartItem>();

        public Cart(IDiscountStore discountStore)
        {
            _discountStore = discountStore ?? throw new ArgumentNullException(nameof(discountStore));
        }

        public IReadOnlyList<CartItem> Items => _items.Values.ToList();

        public void Add(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            if (_items.ContainsKey(product.Name))
            {
                var cartItem = _items[product.Name];
                var updatedCartItem = cartItem.IncreaseQuantity(1);
                _items[product.Name] = updatedCartItem;
            }
            else
            {
                _items[product.Name] = new CartItem(product, 1);
            }
        }

        public void Remove(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (_items.ContainsKey(product.Name) == false)
                return;

            var cartItem = _items[product.Name];
            if (cartItem.Quantity == 1)
            {
                _items.Remove(product.Name);
            }
            else
            {
                var updatedCartItem = cartItem.DecreaseQuantity(1);
                _items[product.Name] = updatedCartItem;
            }
        }

        public Order Checkout()
        {
            var subTotal = _items.Values.Select(cartItem => cartItem.TotalPrice).Sum();
            var discounts = _discountStore.GetDiscounts().SelectMany(d => d.Apply(_items.Values)).ToList();
            var discountsAmount = discounts.Select(d => d.Amount).Sum();
            var total = subTotal - discountsAmount;

            return new Order(subTotal, total, discounts);
        }
    }
}