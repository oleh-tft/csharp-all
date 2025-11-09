using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Notifier.Pages
{
    internal class ProfilePage
    {
        private readonly UserNameStateListener _listener = (username) =>
        {
            Console.WriteLine("ProfilePage got new username: " + username);
        };

        private readonly LastSyncMomentStateListener _lastSyncMomentlistener = (lastSyncMoment) =>
        {
            Console.WriteLine("ProfilePage got new lastSyncMoment: " + lastSyncMoment);
        };

        public ProfilePage()
        {
            Console.WriteLine("ProfilePage starts with username: " + GlobalState.Instance.UserName);
            Console.WriteLine("ProfilePage starts with lastSyncMoment: " + GlobalState.Instance.LastSyncMoment);
            GlobalState.Instance.AddUserNameListener(_listener);
            GlobalState.Instance.AddLastSyncMomentListener(_lastSyncMomentlistener);
        }

        ~ProfilePage()
        {
            GlobalState.Instance.RemoveUserNameListener(_listener);
            GlobalState.Instance.RemoveLastSyncMomentListener(_lastSyncMomentlistener);
        }
    }
}
