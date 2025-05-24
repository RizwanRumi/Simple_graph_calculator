using SimpleGraphCalculatorApp.Interfaces;
using SimpleGraphCalculatorApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Models
{
    public abstract class FunctionFactory
    {
        public abstract BaseFunction CreateFunction(double amplitude, double frequency, double phase);
    }
}
