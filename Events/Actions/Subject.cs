using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events.Actions
{
    internal class Subject
    {
        private static Subject _instance;
        public static Subject Instance => _instance ??= new(0.0);

        public Subject(double initialPrice)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Subject was instantiated already");
            }
            _price = initialPrice;
            _instance = this;
        }

        #region context
        private double _price = 100.0;
        public double Price
        {
            get => _price;
            set
            {
                if (value != _price)
                {
                    _price = value;
                    NotifySubscribers();
                }
            }
        }
        #endregion

        #region Notifier
        private readonly List<Action> subscribers = [];
        

        public void Subscribe(Action action)
        {
            lock (subscribers) { subscribers.Add(action); }
        }
        public void Unsubscribe(Action action)
        {
            lock (subscribers) { subscribers.Remove(action); }
        }
        public void NotifySubscribers()
        {
            lock (subscribers) { subscribers.ForEach(s => s.Invoke()); }
        }
        #endregion
    }
}
