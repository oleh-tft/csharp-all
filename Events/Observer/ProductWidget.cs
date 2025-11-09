using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Observer
{
    internal class ProductWidget
    {
        private readonly ProductWidgetSubscriber subscriber;

        public ProductWidget()
        {
            subscriber = new(OnPriceChanged);
            Publisher.Instance.Subscribe(subscriber);
            Console.WriteLine("Product Widget created with price " + Publisher.Instance.Price);
        }

        public void OnPriceChanged(double price)
        {
            Console.WriteLine("ProductWidget: set new price " + price);
        }

        ~ProductWidget()
        {
            Publisher.Instance.Unsubscribe(subscriber);
        }
    }

    public class ProductWidgetSubscriber(Action<double> action) : ISubscriber
    {
        public void Update(double newPrice)
        {
            action(newPrice);
        }
    }
}
