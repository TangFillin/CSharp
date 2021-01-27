using System;

namespace EncryptAndDecrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            RSACrypt.Test();
            DesCrypt.Test();
            Console.WriteLine("Hello World!");
        }
    }
}
