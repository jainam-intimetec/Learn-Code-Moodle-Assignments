using System;

namespace SubarrayMean.Services
{
    public class ValidationService
    {
        public static bool IsArrayLengthValid(long[] numbers, int expectedLength)
        {
            if (numbers == null || numbers.Length != expectedLength)
            {
                Console.WriteLine("Error: Array Length must be equal to number of elements expected.");
                return false;
            }
            return true;
        }

        public static bool IsQueryRangeValid(int[] range, int arrayLength)
        {
            if (range == null || range.Length < 2) return false;

            int start = range[0]-1;
            int end = range[1]-1;

            if (start > end)
            {
                Console.WriteLine($"Invalid Range: Start index ({start}) cannot be greater than end index ({end}).");
                return false;
            }

            if (start < 0 || end >= arrayLength)
            {
                Console.WriteLine($"Out of Bounds: Range [{start+1}, {end+1}] is outside the array (0 to {arrayLength}).");
                return false;
            }

            return true;
        }
    }
}
