using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverPattern.Version2
{
    public class Subscriber
    {
        public Subscriber(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
        public void ReceiveAndPrint(object obj)
        {
            TenXun tx = obj as TenXun;
            if (tx != null)
            {
                Console.WriteLine($"Nofified {Name} of {tx.Symbol}'s Info is:{tx.Info}");
            }
            
        }
    }
}
