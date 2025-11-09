using csharp_all.Events.Notifier.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Notifier
{
    internal class NotifierDemo
    {
        public void Run()
        {
            GlobalState globalState = new(100.0, "Default", "Home");
            new PricePage();
            new ProfilePage();
            new RouterModule();
            globalState.Price = 200.0;
            globalState.UserName = "User";
            globalState.LastSyncMoment = DateTime.Now;
            globalState.ActivePage = "Profile";

        }
    }
}
