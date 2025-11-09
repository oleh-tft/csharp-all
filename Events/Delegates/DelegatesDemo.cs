using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Delegates
{
    internal class DelegatesDemo
    {
        public void Run()
        {
            State state = new(100.0);
            new PriceView();
            new CartView();
            new ProductView();
            state.Price = 200.0;
        }
    }
}
