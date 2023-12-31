
using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome6181();
            Welcome8941();

        }
        static partial void Welcome8941();
        private static void Welcome6181()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine(name + ", welcome to my first console application");
        }
    }
}
