using System;
using DivisorApp.Core.Interfaces;

namespace DivisorApp.Infrastructure.Math
{
    public class DivisorCalculator : IDivisorCalculator
    {
        public int GetDivisorCount(int number)
        {
            if (number <= 0)
                throw new ArgumentException("Number must be positive");

            int count = 0;
            int sqrt = (int)System.Math.Sqrt(number);

            for (int i = 1; i <= sqrt; i++)
            {
                if (number % i == 0)
                {
                    count += (i * i == number) ? 1 : 2;
                }
            }

            return count;
        }
    }
}