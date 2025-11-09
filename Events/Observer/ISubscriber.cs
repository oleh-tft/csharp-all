using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Observer
{
    internal interface ISubscriber
    {
        void Update(double newPrice)
        {

        }
    }
}
