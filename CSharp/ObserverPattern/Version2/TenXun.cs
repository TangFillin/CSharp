using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverPattern.Version2
{
    public class TenXun
    {
        public TenXun(string symbol, string info)
        {
            this.Symbol = symbol;
            this.Info = info;
        }
        //保存订阅者列表
        private NotifyEventHandler NotifyEvent;

        public string Symbol { get; set; }
        public string Info { get; set; }
        #region 对订阅号列表的维护
        public void AddObserver(NotifyEventHandler ob)
        {
            NotifyEvent += ob;
        }
        public void RemoveObserver(NotifyEventHandler ob)
        {
            NotifyEvent -= ob;
        }
        #endregion

        public void Update()
        {

            NotifyEvent?.Invoke(this);
        }
    }
}
