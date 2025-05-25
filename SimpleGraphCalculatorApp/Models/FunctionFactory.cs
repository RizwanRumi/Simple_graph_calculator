namespace SimpleGraphCalculatorApp.Models
{
    public abstract class FunctionFactory
    {
        public abstract BaseFunction CreateFunction(double amplitude, double frequency, double phase);
    }
}
