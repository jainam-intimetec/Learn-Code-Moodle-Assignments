using DivisorApp.Core.Interfaces;

namespace DivisorApp.Application.Services
{
    public class ConsecutiveDivisorService
    {
        private readonly IDivisorCalculator _calculator;

        public ConsecutiveDivisorService(IDivisorCalculator calculator)
        {
            _calculator = calculator;
        }

        public int CountValidNumbers(int limit)
        {
            if (limit <= 2)
                return 0;

            int count = 0;

            for (int n = 2; n < limit; n++)
            {
                if (HasEqualDivisors(n, n + 1))
                {
                    count++;
                }
            }

            return count;
        }

        private bool HasEqualDivisors(int first, int second)
        {
            return _calculator.GetDivisorCount(first) ==
                   _calculator.GetDivisorCount(second);
        }
    }
}