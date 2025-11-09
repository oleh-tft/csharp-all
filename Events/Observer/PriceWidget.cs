using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Observer
{
    internal class PriceWidget
    {
        private readonly PriceWidgetSubscriber subscriber;

        public PriceWidget()
        {
            subscriber = new(OnPriceChanged);
            Publisher.Instance.Subscribe(subscriber);
            Console.WriteLine("Price Widget created with price " + Publisher.Instance.Price);
        }

        public void OnPriceChanged(double price)
        {
            Console.WriteLine("PriceWidget: set new price " + price);
        }

        ~PriceWidget()
        {
            Publisher.Instance.Unsubscribe(subscriber);
        }
    }

    public class PriceWidgetSubscriber(Action<double> action) : ISubscriber
    {
        public void Update(double newPrice)
        {
            action(newPrice);
        }
    }
}
    