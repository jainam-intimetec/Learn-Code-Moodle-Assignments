using SubarrayMean.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubarrayMean
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mathService = new FloorMeanService();
            var app = new AppRunner(mathService);

            app.Start();
        }
    }
}
