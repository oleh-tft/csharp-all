using csharp_all.Events.Actions;
using csharp_all.Events.Delegates;
using csharp_all.Events.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Events
{
    internal class EventsDemo
    {
        public void Run()
        {
            Console.WriteLine("Events Demo");
            //new ObserverDemo().Run();
            //new ActionsDemo().Run();
            //new DelegatesDemo().Run();
            Emitter emitter = new(initialPrice: 100.0);
            PriceComponent pc = new();
            CartComponent cc = new();
            ProductComponent prc = new();
            Console.WriteLine("----------------");
            emitter.Price = 200.0;
        }
    }

    public delegate void EmitterListener(double price);

    class Emitter
    {
        private event EmitterListener? EmitterEvent;
        public void Subscribe(EmitterListener listener) => EmitterEvent += listener;
        public void Unsubscribe(EmitterListener listener) => EmitterEvent -= listener; 
        
        private static Emitter? _instance = null;
        public static Emitter Instance => _instance ??= new(0.0);

        public Emitter(double initialPrice)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Emitter was instantiated already");
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
                    EmitterEvent?.Invoke(_price);
                }
            }
        }
        #endregion
    }

    class PriceComponent
    {
        private readonly EmitterListener _listener = (price) =>
        {
            Console.WriteLine("PriceComponent got new price: " + price);
        };

        public PriceComponent()
        {
            Console.WriteLine("PriceComponent starts with price: " + Emitter.Instance.Price);
            Emitter.Instance.Subscribe(_listener);
        }

        ~PriceComponent()
        {
            Emitter.Instance.Unsubscribe(_listener);
        }
    }

    class CartComponent
    {
        private readonly EmitterListener _listener = (price) =>
        {
            Console.WriteLine("CartComponent got new price: " + price);
        };

        public CartComponent()
        {
            Console.WriteLine("CartComponent starts with price: " + Emitter.Instance.Price);
            Emitter.Instance.Subscribe(_listener);
        }

        ~CartComponent()
        {
            Emitter.Instance.Unsubscribe(_listener);
        }
    }

    class ProductComponent
    {
        private readonly EmitterListener _listener = (price) =>
        {
            Console.WriteLine("ProductComponent got new price: " + price);
        };

        public ProductComponent()
        {
            Console.WriteLine("ProductComponent starts with price: " + Emitter.Instance.Price);
            Emitter.Instance.Subscribe(_listener);
        }

        ~ProductComponent()
        {
            Emitter.Instance.Unsubscribe(_listener);
        }
    }
}
