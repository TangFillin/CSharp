using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverPattern.Version1
{
    /// <summary>
    /// 订阅者接口
    /// </summary>
    public interface IObserver
    {
        void ReceiveAndPrint(TenXun tx);
    }
}
