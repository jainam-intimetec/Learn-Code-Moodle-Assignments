using SubarrayMean.Entities;
using SubarrayMean.Interfaces;
using SubarrayMean.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubarrayMean
{
    public class AppRunner
    {
        private readonly IMeanCalculator _calculator;

        public AppRunner(IMeanCalculator calculator) => _calculator = calculator;

        public void Start()
        {
            try
            {
                RunProcess();
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void RunProcess()
        {
            var config = ReadInt(); 
            int numberOfElements  = config[0];
            int numberOfQueries  = config[1];
            var numbers = ReadLong();
            if (!ValidationService.IsArrayLengthValid(numbers, numberOfElements)) return;

            var table = new PrefixSumTable(numbers);

            for (int query = 0; query < numberOfQueries; query++)
            {
                var range = ReadInt();
                if (!ValidationService.IsQueryRangeValid(range, numberOfElements)) continue;
                long sum = table.GetRangeSum(range[0], range[1]);
                int count = range[1] - range[0] + 1;
                Console.WriteLine(_calculator.GetMean(sum, count));
            }
        }

        private int[] ReadInt() => Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
        private long[] ReadLong() => Array.ConvertAll(Console.ReadLine().Split(' '), long.Parse);
    }
}
