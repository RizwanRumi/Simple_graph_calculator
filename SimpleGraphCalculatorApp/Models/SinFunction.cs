using SimpleGraphCalculatorApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Models
{
    public class SinFunction : BaseFunction
    {
        public SinFunction(double amplitude, double frequency, double phase) : base(amplitude, frequency, phase)
        {
            Amplitude = amplitude;
            Frequency = frequency;
            Phase = phase;
        }

        public override double Calculate(double x)
        {
            return Amplitude * Math.Sin(Frequency * x + Phase);
        }
    }
}
