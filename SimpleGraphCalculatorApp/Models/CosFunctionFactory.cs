using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Models
{
    public class CosFunctionFactory : FunctionFactory
    {
        public override BaseFunction CreateFunction(double amplitude, double frequency, double phase)
        {
            return new CosFunction(amplitude, frequency, phase);
        }
    }
}
