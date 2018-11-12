using System;
using System.Linq;
using PCT.Shop.Core.Domain;
using PCT.Shop.Data;

namespace PCT.Shop.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var productStore = new InMemoryProductStore();
            var discountStore = new InMemoryDiscountStore();
            var cart = new Cart(discountStore);
            var printer = new OrderSummaryPrinter();

            var products = productStore.GetProducts();

            foreach (var productName in args)
            {
                var product = products.SingleOrDefault(p =>
                    p.Name.Equals(productName.Trim(), StringComparison.CurrentCultureIgnoreCase));

                if (product == null)
                {
                    PrintProductNotFound(productName);
                }
                else
                {
                    cart.Add(product);
                }
            }

            var order = cart.Checkout();
            printer.PrintOrderSummary(order);

            Console.WriteLine("Press any key to exit ...");
            Console.ReadLine();
        }

        private static void PrintProductNotFound(string productName)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"'{productName}' is NOT a valid product name and will be skipped!");
            Console.ResetColor();
        }
    }
}
