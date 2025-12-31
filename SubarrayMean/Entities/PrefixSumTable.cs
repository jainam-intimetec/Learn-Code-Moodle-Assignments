using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubarrayMean.Entities
{
    public class PrefixSumTable
    {
        private readonly long[] _prefixSum;

        public PrefixSumTable(long[] numbers)
        {
            _prefixSum = BuildTable(numbers);
        }

        private long[] BuildTable(long[] numbers)
        {
            long[] table = new long[numbers.Length + 1];
            for (int i = 0; i < numbers.Length; i++)
                table[i + 1] = table[i] + numbers[i];
            return table;
        }

        public long GetRangeSum(int start, int end)
        {
            return _prefixSum[end] - _prefixSum[start - 1];
        }
    }
}
