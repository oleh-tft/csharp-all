using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Notifier
{
    public delegate void PriceStateListener(double price);
    public delegate void UserNameStateListener(String username);
    public delegate void ActivePageStateListener(String activePage);
    public delegate void LastSyncMomentStateListener(DateTime dateTime);

    internal class GlobalState
    {
        private static GlobalState? _instance = null;
        public static GlobalState Instance => _instance ??= new(0.0, "", "Home");

        public GlobalState(double price, String username, String activePage)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("GlobalState was instantiated already");
            }
            _price = price;
            _userName = username;
            _activePage = activePage;
            _lastSyncMoment = new DateTime(2025, 11, 8);
            _instance = this;
        }

        private double _price;
        private String _userName;
        private String _activePage;
        private DateTime _lastSyncMoment;

        public double Price
        {
            get => _price;
            set
            {
                if (value != _price)
                {
                    _price = value;
                    NotifyPriceListeners(_price);
                }
            }
        }
        public DateTime LastSyncMoment
        {
            get => _lastSyncMoment;
            set
            {
                if (value != _lastSyncMoment)
                {
                    _lastSyncMoment = value;
                    NotifyLastSyncMomentListeners(_lastSyncMoment);
                }
            }
        }
        public String? UserName
        {
            get => _userName;
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    NotifyUserNameListeners(_userName);
                }
            }
        }
        public String ActivePage
        {
            get => _activePage;
            set
            {
                if (value != _activePage)
                {
                    _activePage = value;
                    NotifyActivePageListeners(_activePage);
                }
            }
        }

        private readonly List<PriceStateListener> priceListeners = [];
        private readonly List<UserNameStateListener> usernameListeners = [];
        private readonly List<ActivePageStateListener> activePageListeners = [];
        private readonly List<LastSyncMomentStateListener> lastSyncMomentListeners = [];

        public void AddPriceListener(PriceStateListener listener) => priceListeners.Add(listener);
        public void RemovePriceListener(PriceStateListener listener) => priceListeners.Remove(listener);
        private void NotifyPriceListeners(double price) =>
            priceListeners.ForEach(listener => listener(price));

        public void AddUserNameListener(UserNameStateListener listener) => usernameListeners.Add(listener);
        public void RemoveUserNameListener(UserNameStateListener listener) => usernameListeners.Remove(listener);
        private void NotifyUserNameListeners(String username) =>
            usernameListeners.ForEach(listener => listener(username));

        public void AddActivePageListener(ActivePageStateListener listener) => activePageListeners.Add(listener);
        public void RemoveActivePageListener(ActivePageStateListener listener) => activePageListeners.Remove(listener);
        private void NotifyActivePageListeners(String activePage) =>
            activePageListeners.ForEach(listener => listener(activePage));

        public void AddLastSyncMomentListener(LastSyncMomentStateListener listener) => lastSyncMomentListeners.Add(listener);
        public void RemoveLastSyncMomentListener(LastSyncMomentStateListener listener) => lastSyncMomentListeners.Remove(listener);
        private void NotifyLastSyncMomentListeners(DateTime lastSyncMoment) =>
            lastSyncMomentListeners.ForEach(listener => listener(lastSyncMoment));
    }
}
