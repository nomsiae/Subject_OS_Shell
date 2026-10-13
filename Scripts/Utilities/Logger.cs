using System;
using System.Diagnostics;

namespace OS_Shell
{
    public static class Logger
    {
        public static void Log(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }

        public static void DeLog(string message)
        {
            Debug.WriteLine($"[Debug] {message}");
        }
    }   
}
