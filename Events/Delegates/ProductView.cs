using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Delegates
{
    internal class ProductView
    {
        private readonly StateListener _listener = (price) =>
        {
            Console.WriteLine("ProductView got new price: " + price);
        };

        public ProductView()
        {
            Console.WriteLine("ProductView starts with price: " + State.Instance.Price);
            State.Instance.AddListener(_listener);
        }

        ~ProductView()
        {
            State.Instance.RemoveListener(_listener);
        }
    }
}
