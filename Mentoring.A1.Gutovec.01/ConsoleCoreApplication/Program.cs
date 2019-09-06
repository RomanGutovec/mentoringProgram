using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ConsoleCoreApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine(HelloWithTimeLib.HelloWithTime.GetHelloWithTime(args[0]));
            }
        }
    }
}
