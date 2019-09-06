using System;

namespace HelloWithTimeLib
{
    public class HelloWithTime
    {
        public static string GetHelloWithTime(string name)
            => $"{DateTime.Now.Hour}:{DateTime.Now.Minute} Hello, {name}!";
    }
}
