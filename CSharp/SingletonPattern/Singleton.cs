using System;

namespace SingletonPattern
{
    public class Singleton
    {
        private static Singleton Singlet = null;
        /// <summary>
        /// 最简单的懒汉模式，线程不安全
        /// </summary>
        private Singleton() { }
        public static Singleton GetInstance()
        {
            if (Singlet == null)
            {
                Singlet = new Singleton();
            }
            return Singlet;
        }
    }
    public class Singleton1
    {
        /// <summary>
        /// 懒汉模式，线程安全，性能低
        /// </summary>
        private static Singleton1 Singlet = null;
        //线程间共享对象
        private static readonly object obj = new object();
        /// <summary>
        /// 最简单的懒汉模式，线程不安全
        /// </summary>
        private Singleton1() { }
        public static Singleton1 GetInstance()
        {
            lock (obj)
            {
                if (Singlet == null)
                {
                    Singlet = new Singleton1();
                }
                return Singlet;
            }
            
        }
    }
    public sealed class Singleton3
    {
        private static Singleton3 instance = null;
        private static readonly object obj = new object();

        private Singleton3() { }

        public static Singleton3 Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new Singleton3();
                        }
                    }
                }
                return instance;
            }
        }
        /// <summary>
        /// 建议
        /// </summary>
        public sealed class Singleton4
        {
            private static readonly Singleton4 instance = new Singleton4();

            /// <summary>
            /// 显式的静态构造函数用来告诉C#编译器在其内容实例化之前不要标记其类型
            /// </summary>
            static Singleton4() { }

            private Singleton4() { }

            public static Singleton4 Instance
            {
                get
                {
                    return instance;
                }
            }
        }
        /// <summary>
        /// 建议
        /// </summary>
        public sealed class Singleton5
        {
            private Singleton5() { }

            public static Singleton5 Instance { get { return Nested.instance; } }

            private class Nested
            {
                //Explicit static constructor to tell C# compiler
                //not to mark type as beforefieldinit
                static Nested()
                {

                }

                internal static readonly Singleton5 instance = new Singleton5();
            }

        }
        public sealed class Singleton6
        {
            private static readonly Lazy<Singleton6> lazy =
                   new Lazy<Singleton6>(() => new Singleton6());

            public static Singleton6 Instance { get { return lazy.Value; } }

            private Singleton6() { }
        }
    }
}
