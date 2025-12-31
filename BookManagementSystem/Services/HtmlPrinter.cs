using System;
using BookSystem.Interfaces;

namespace BookSystem.Services
{
    public class HtmlPrinter : IPrinter
    {
        public void PrintPage(string content)
        {
            Console.WriteLine($"<div class='page'>{content}</div>");
        }
    }
}