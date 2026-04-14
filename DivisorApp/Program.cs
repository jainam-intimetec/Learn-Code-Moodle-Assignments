using System;
using DivisorApp.Application.Services;
using DivisorApp.Infrastructure;
using DivisorApp.Infrastructure.Math;

class Program
{
    static void Main()
    {
        var calculator = new DivisorCalculator();
        var service = new ConsecutiveDivisorService(calculator);

        Console.Write("Enter number of test cases: ");
        int t = ConsoleInputReader.ReadRequiredInt(Console.In);

        for (int i = 0; i < t; i++)
        {
            Console.Write("Enter k: ");
            int k = ConsoleInputReader.ReadRequiredInt(Console.In);

            int result = service.CountValidNumbers(k);
            Console.WriteLine(result);
        }
    }
}
