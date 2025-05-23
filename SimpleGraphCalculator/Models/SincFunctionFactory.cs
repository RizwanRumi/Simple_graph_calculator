using SimpleGraphCalculatorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculator.Models
{
    public class SincFunctionFactory : FunctionFactory
    {
        public override BaseFunction CreateFunction(double amplitude, double frequency, double phase)
        {
            return new SincFunction(amplitude, frequency, phase);
        }
    }
}
