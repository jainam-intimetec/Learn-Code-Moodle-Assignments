using SubarrayMean.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubarrayMean.Services
{
    public class FloorMeanService : IMeanCalculator
    {
        public long GetMean(long sum, int numberOfElements)
        {
            return numberOfElements == 0 ? 0 : sum / numberOfElements;
        }
    }
}
