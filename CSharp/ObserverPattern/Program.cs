using ObserverPattern.Version1;
using System;

namespace ObserverPattern
{
    /// <summary>
    /// 观察者模式
    /// 抽象主题角色（Subject）：抽象主题把所有观察者对象的引用保存在一个列表中，并提供增加和删除观察者对象的操作，抽象主题角色又叫做抽象被观察者角色，一般由抽象类或接口实现。
    /// 抽象观察者角色（Observer）：为所有具体观察者定义一个接口，在得到主题通知时更新自己，一般由抽象类或接口实现。
    /// 具体主题角色（ConcreteSubject）：实现抽象主题接口，具体主题角色又叫做具体被观察者角色。
    /// 具体观察者角色（ConcreteObserver）：实现抽象观察者角色所要求的接口，以便使自身状态与主题的状态相协调。
    ///
    /// 观察者模式有以下几个优点：
    /// 观察者模式实现了表示层和数据逻辑层的分离，并定义了稳定的更新消息传递机制，并抽象了更新接口，使得可以有各种各样不同的表示层，即观察者。
    /// 观察者模式在被观察者和观察者之间建立了一个抽象的耦合，被观察者并不知道任何一个具体的观察者，只是保存着抽象观察者的列表，每个具体观察者都符合一个抽象观察者的接口。
    /// 观察者模式支持广播通信。被观察者会向所有的注册过的观察者发出通知。
    ///
    ///观察者也存在以下一些缺点：
    ///如果一个被观察者有很多直接和间接的观察者时，将所有的观察者都通知到会花费很多时间。
    ///虽然观察者模式可以随时使观察者知道所观察的对象发送了变化，但是观察者模式没有相应的机制使观察者知道所观察的对象是怎样发生变化的。
    ///如果在被观察者之间有循环依赖的话，被观察者会触发它们之间进行循环调用，导致系统崩溃，在使用观察者模式应特别注意这点。
    /// </summary>

    class Program
    {
        
        static void Main(string[] args)
        {
            #region 1.简单不完善版
            //Subscriber subscriber = new Subscriber("LearningHard");
            //TenxunGame tx = new TenxunGame();

            //tx.Subscriber = subscriber;
            //tx.Symbol = "TenXun Game";
            //tx.Info = "Have a new game published ...";

            //tx.Update();
            #endregion

            #region 2.完整版Version1
            //TenXun tenXun = new Version1.TenXunGame("TenXun Game", "Have a new game Pulised.");

            //tenXun.AddObserver(new Version1.Subscriber("Learning Hard"));
            //tenXun.AddObserver(new Version1.Subscriber("Fillin"));

            //tenXun.Update();
            #endregion

            #region 3. .Net版本，使用委托简化
            Version2.TenXun tenXun = new Version2.TenXunGame("TenXun Game", "Have a new game Pulised.");
            Version2.Subscriber lh = new Version2.Subscriber("Learning Hard");
            Version2.Subscriber fillin = new Version2.Subscriber("Fillin");
            tenXun.AddObserver(new NotifyEventHandler(lh.ReceiveAndPrint));
            tenXun.AddObserver(new NotifyEventHandler(fillin.ReceiveAndPrint));

            tenXun.Update();
            #endregion
            Console.ReadKey();
        }
    }
    //委托充当订阅者接口
    public delegate void NotifyEventHandler(object sender);

    public class TenxunGame
    {
        public Subscriber Subscriber { get; set; }
        public string Symbol { get; set; }
        public string Info { get; set; }
        
        public void Update()
        {
            if(Subscriber!=null)
            {
                Subscriber.ReceiveAndPrintData(this);
            }
        }
    }
    /// <summary>
    /// 订阅者（观察者）
    /// </summary>
    public class Subscriber
    {
        public string Name { get; set; }
        public Subscriber(string name)
        {
            this.Name = name;
        }
        public void ReceiveAndPrintData(TenxunGame tx)
        {
            Console.WriteLine($"Nofified {Name} of {tx.Symbol}'s Info is:{tx.Info}");
        }
    }
}
