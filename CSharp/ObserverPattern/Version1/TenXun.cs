using System.Collections.Generic;

namespace ObserverPattern.Version1
{
    //订阅号抽象类
    public abstract class TenXun
    {
        public TenXun(string symbol, string info)
        {
            this.Symbol = symbol;
            this.Info = info;
        }
        //保存订阅者列表
        private List<IObserver> Observers = new List<IObserver>();

        public string Symbol { get; set; }
        public string Info { get; set; }
        #region 对订阅号列表的维护
        public void AddObserver(IObserver ob)
        {
            Observers.Add(ob);
        }
        public void RemoveObserver(IObserver ob)
        {
            Observers.Remove(ob);
        }
        #endregion

        public void Update()
        {
            foreach (var ob in Observers)
            {
                if(ob!=null)
                {
                    ob.ReceiveAndPrint(this);
                }
            }
        }

    }
}
