using System;
using System.Linq;
using PCT.Shop.Core.Domain;

namespace PCT.Shop.ConsoleApp
{
    public sealed class OrderSummaryPrinter
    {
        public void PrintOrderSummary(Order order)
        {
            Console.WriteLine("");
            Console.WriteLine(new String('*', 20));
            Console.WriteLine("");
            Console.WriteLine($"Subtotal: {order.SubTotal:C2}");

            if (order.Discounts.Any())
            {
                foreach (var discount in order.Discounts)
                {
                    Console.WriteLine(discount.Description);
                }
            }
            else
            {
                Console.WriteLine("(no offers available)");
            }
            Console.WriteLine($"Total: {order.Total:C2}");
            Console.WriteLine("");
            Console.WriteLine(new String('*', 20));
        }
    }
}