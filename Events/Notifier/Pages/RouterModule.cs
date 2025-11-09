using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Notifier.Pages
{
    internal class RouterModule
    {
        private readonly ActivePageStateListener _listener = (activePage) =>
        {
            Console.WriteLine("RouterModule got new activePage: " + activePage);
        };

        private readonly LastSyncMomentStateListener _lastSyncMomentlistener = (lastSyncMoment) =>
        {
            Console.WriteLine("RouterModule got new lastSyncMoment: " + lastSyncMoment);
        };

        public RouterModule()
        {
            Console.WriteLine("RouterModule starts with activePage: " + GlobalState.Instance.ActivePage);
            Console.WriteLine("RouterModule starts with lastSyncMoment: " + GlobalState.Instance.LastSyncMoment);
            GlobalState.Instance.AddActivePageListener(_listener);
            GlobalState.Instance.AddLastSyncMomentListener(_lastSyncMomentlistener);
        }

        ~RouterModule()
        {
            GlobalState.Instance.AddActivePageListener(_listener);
            GlobalState.Instance.RemoveLastSyncMomentListener(_lastSyncMomentlistener);
        }
    }
}
