using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Models
{
    public class CosFunction : BaseFunction
    {
        public CosFunction(double amplitude, double frequency, double phase) : base(amplitude, frequency, phase)
        {
            Amplitude = amplitude;
            Frequency = frequency;
            Phase = phase;
        }

        public override double Calculate(double x)
        {
            return Amplitude * Math.Cos(Frequency * x + Phase);
        }
    }
}
