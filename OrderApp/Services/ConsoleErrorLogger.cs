using System;
using OrderApp.Interfaces;

namespace OrderApp.Services
{
    public class ConsoleErrorLogger : IErrorLogger
    {
        public void Log(Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
