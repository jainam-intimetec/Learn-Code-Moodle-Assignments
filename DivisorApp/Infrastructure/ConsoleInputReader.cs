using System;
using System.IO;

namespace DivisorApp.Infrastructure
{
    public static class ConsoleInputReader
    {
        public static int ReadRequiredInt(TextReader reader)
        {
            string? input = reader.ReadLine();
            return int.Parse(input);
        }
    }
}
