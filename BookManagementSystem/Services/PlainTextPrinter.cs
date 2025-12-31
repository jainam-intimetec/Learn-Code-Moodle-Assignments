using System;
using BookSystem.Interfaces;

namespace BookSystem.Services
{
    public class PlainTextPrinter : IPrinter 
    {
        public void PrintPage(string page) 
        {
            Console.WriteLine(page);
        }
    }
}