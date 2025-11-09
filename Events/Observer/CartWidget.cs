using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Observer
{
    internal class CartWidget
    {
        private readonly CartWidgetSubscriber subscriber;

        public CartWidget()
        {
            subscriber = new(OnPriceChanged);
            Publisher.Instance.Subscribe(subscriber);
            Console.WriteLine("Cart Widget created with price " + Publisher.Instance.Price);
        }

        public void OnPriceChanged(double price)
        {
            Console.WriteLine("CartWidget: set new price " + price);
        }

        ~CartWidget()
        {
            Publisher.Instance.Unsubscribe(subscriber);
        }
    }

    public class CartWidgetSubscriber(Action<double> action) : ISubscriber
    {
        public void Update(double newPrice)
        {
            action(newPrice);
        }
    }
}
