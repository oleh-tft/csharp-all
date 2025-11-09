using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Notifier.Pages
{
    internal class PricePage
    {
        private readonly PriceStateListener _listener = (price) =>
        {
            Console.WriteLine("PricePage got new price: " + price);
        };

        private readonly LastSyncMomentStateListener _lastSyncMomentlistener = (lastSyncMoment) =>
        {
            Console.WriteLine("PricePage got new lastSyncMoment: " + lastSyncMoment);
        };

        public PricePage()
        {
            Console.WriteLine("PricePage starts with price: " + GlobalState.Instance.Price);
            Console.WriteLine("PricePage starts with lastSyncMoment: " + GlobalState.Instance.LastSyncMoment);
            GlobalState.Instance.AddPriceListener(_listener);
            GlobalState.Instance.AddLastSyncMomentListener(_lastSyncMomentlistener);
        }

        ~PricePage()
        {
            GlobalState.Instance.RemovePriceListener(_listener);
            GlobalState.Instance.RemoveLastSyncMomentListener(_lastSyncMomentlistener);
        }
    }
}
