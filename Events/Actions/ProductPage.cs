using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Actions
{
    internal class ProductPage
    {
        public ProductPage()
        {
            Console.WriteLine("ProductPage shown price: " + Subject.Instance.Price);
            Subject.Instance.Subscribe(OnPriceChanged);
        }

        private void OnPriceChanged()
        {
            Console.WriteLine("ProductPage got new price: " + Subject.Instance.Price);
        }

        ~ProductPage()
        {
            Subject.Instance.Unsubscribe(OnPriceChanged);
        }
    }
}
