using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Delegates
{
    internal class PriceView
    {
        private readonly StateListener _listener = (price) =>
        {
            Console.WriteLine("PriceView got new price: " + price);
        };

        public PriceView()
        {
            Console.WriteLine("PriceView starts with price: " + State.Instance.Price);
            State.Instance.AddListener(_listener);
        }

        ~PriceView()
        {
            State.Instance.RemoveListener(_listener);
        }
    }
}
