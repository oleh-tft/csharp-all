using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Delegates
{
    internal class CartView
    {
        private readonly StateListener _listener = (price) =>
        {
            Console.WriteLine("CartView got new price: " + price);
        };

        public CartView()
        {
            Console.WriteLine("CartView starts with price: " + State.Instance.Price);
            State.Instance.AddListener(_listener);
        }

        ~CartView()
        {
            State.Instance.RemoveListener(_listener);
        }
    }
}
