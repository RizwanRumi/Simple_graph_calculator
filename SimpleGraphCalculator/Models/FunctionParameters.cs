using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Models
{   
    public enum FunctionType 
    { 
        Sin, 
        Cos, 
        Sinc
    }

    public class FunctionParameters
    {
        public FunctionType Type { get; set; } = FunctionType.Sin;
        public double Amplitude { get; set; } = 1.0;
        public double Frequency { get; set; } = 1.0;
        public double Phase { get; set; } = 0.0;
        public double RangeStart { get; set; } = -10.0;
        public double RangeEnd { get; set; } = 10.0;
    }
}
