using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverPattern.Version1
{
    public class Subscriber : IObserver
    {
        public Subscriber(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
        public void ReceiveAndPrint(TenXun tx)
        {
            Console.WriteLine($"Nofified {Name} of {tx.Symbol}'s Info is:{tx.Info}");
        }
    }
}
