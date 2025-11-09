using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Observer
{
    internal class ObserverDemo
    {
        public void Run()
        {
            PriceWidget pw = new();
            CartWidget cw = new();
            ProductWidget prw = new();

            Publisher.Instance.Price = 200.0;
        }
    }
}
