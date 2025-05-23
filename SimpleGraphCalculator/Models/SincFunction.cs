using SimpleGraphCalculatorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculator.Models
{
    public class SincFunction : BaseFunction
    {
        public SincFunction(double amplitude, double frequency, double phase) : base(amplitude, frequency, phase)
        {
            Amplitude = amplitude;
            Frequency = frequency;
            Phase = phase;
        }

        public override double Calculate(double x)
        {            
            double value = Frequency * x + Phase;

            // Handle division by zero for sinc function   
            if (Math.Abs(value) < 1e-10)
                return Amplitude;

            return Amplitude * (Math.Sin(value) / value);
        }
    }
}
