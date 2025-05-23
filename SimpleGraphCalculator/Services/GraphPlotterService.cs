using SimpleGraphCalculatorApp.Interfaces;
using SimpleGraphCalculatorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Services
{
    public class GraphPlotterService
    {
        private readonly IFunction _currentFunction;

        // Constructor injection   
        public GraphPlotterService(FunctionFactory factory, double amplitude, double frequency, double phase)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _currentFunction = factory.CreateFunction(amplitude, frequency, phase);
        }
                
        public double CalculateFunctionValue(double x)
        {
            if (_currentFunction == null)
                throw new InvalidOperationException("Function is not initialized.");

            return _currentFunction.Calculate(x);
        }
    }
}
