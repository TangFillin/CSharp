using System;

namespace DelegateExample
{
    class Program
    {
        public delegate void Say(string name);

        public delegate string Convert(string str);

        public delegate bool Compare<T>(T a, T b);

        public delegate void ShowMessage();
        static void Main(string[] args)
        {
            //委托
            //Say say = SayHello;
            //say("world");

            //匿名函数
            Say say = delegate (string name) { };

            //lamba表达式
            string[] strs = { "Acb", "dEfgHII", "LDMd3", "0d.dEDE" };
            StringConvert(strs, (string str) => { return str.ToUpper(); });
            foreach (var str in strs)
            {
                Console.WriteLine(str);
            }

            //泛型委托
            int[] objs = { 10, 39, 10, 56, 82, 100 };
            var result = GetMax(objs, (int a, int b) => { return a < b; });
            var result1 = GetMax(strs, (string a, string b) => { return a.CompareTo(b)<0; });
            Console.WriteLine("Max:" + result1);

            //多播委托
            ShowMessage show = () => { Console.WriteLine("Hello"); };
            show += () => { Console.WriteLine("Hi"); };

            show();



            //无返回值委托,最多16个参数
            Action action0 = () => Console.WriteLine("1231");
            Action<int,string> action1=(age,name)=>Console.WriteLine($"{age},{name}");
            action1.Invoke(20, "fillin");

            //有返回值 0-16个参数 泛型委托
            Func<int> func0 = () => DateTime.Now.Year;
            var year = func0.Invoke();
            Func<int, int, int> func1 = (a, b) => a + b;
            var sum = func1.Invoke(1, 1);


        }

        public static T GetMax<T>(T[] objs, Compare<T> compare)
        {
            var max = objs[0];
            foreach (var item in objs)
            {
                if (compare(max, item))
                {
                    max = item;
                }
            }
            return max;
        }

        public static void StringConvert(string[] strs, Convert convert)
        {
            for (var i = 0; i < strs.Length; i++)
            {
                strs[i] = convert(strs[i]);
            }
        }
        public static void SayHello(string name)
        {
            Console.WriteLine($"Hello, {name}");
        }
    }
}
