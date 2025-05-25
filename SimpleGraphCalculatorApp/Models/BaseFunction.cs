using SimpleGraphCalculatorApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SimpleGraphCalculatorApp.Models
{
    public abstract class BaseFunction : IFunction
    {
        public double Amplitude { get; set; }
        public double Frequency { get; set; }
        public double Phase { get; set; }

        // Parameterless constructor for Moq
        protected BaseFunction()
        {           
            Amplitude = 1.0;
            Frequency = 1.0;
            Phase = 0.0;
        }

        protected BaseFunction(double amplitude, double frequency, double phase)
        {
            Amplitude = amplitude;
            Frequency = frequency;
            Phase = phase;
        }

        public abstract double Calculate(double x);       
    }
}
