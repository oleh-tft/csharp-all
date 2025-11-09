using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Actions
{
    internal class ActionsDemo
    {
        public void Run()
        {
            Subject subject = new(initialPrice: 100.0);
            PricePage pricePage = new();
            CartPage cartPage = new();
            ProductPage productPage = new();
            Console.WriteLine("----------------");
            subject.Price = 200.0;
        }
    }
}
