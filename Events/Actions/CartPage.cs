using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Actions
{
    internal class CartPage
    {
        public CartPage()
        {
            Console.WriteLine("CartPage shown price: " + Subject.Instance.Price);
            Subject.Instance.Subscribe(OnPriceChanged);
        }

        private void OnPriceChanged()
        {
            Console.WriteLine("CartPage got new price: " + Subject.Instance.Price);
        }

        ~CartPage()
        {
            Subject.Instance.Unsubscribe(OnPriceChanged);
        }
    }
}
