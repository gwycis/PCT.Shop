using System;

namespace PCT.Shop.Core.Domain
{
    public sealed class CartItem
    {
        private readonly Product _product;

        public string Name => _product.Name;
        public decimal UnitPrice => _product.Price;
        public int Quantity { get; }
        public decimal TotalPrice => UnitPrice * Quantity;
        


        public CartItem(Product product, int quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            Quantity = quantity;
            _product = product ?? throw new ArgumentNullException(nameof(product));
        }

        public CartItem IncreaseQuantity(int quantityIncrease)
        {
            var newQuantity = Quantity + quantityIncrease;
            return new CartItem(_product, newQuantity);
        }

        public CartItem DecreaseQuantity(int quantityDecrease)
        {
            var newQuantity = Quantity - quantityDecrease;
            if (newQuantity < 1)
                newQuantity = 1;

            return new CartItem(_product, newQuantity);
        }
    }
}