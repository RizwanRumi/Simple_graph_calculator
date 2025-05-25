using NUnit.Framework;
using SimpleGraphCalculatorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Test
{
    [TestFixture]
    public class FunctionTests
    {
        private SinFunction _sinFunction;
        private CosFunction _cosFunction;
        private SincFunction _sincFunction;
                

        [Test]
        [TestCase(1.0, 1.0, 0.0, 0.0, 0.0)]
        [TestCase(1.0, 1.0, 0.0, Math.PI / 2, 1.0)]
        [TestCase(1.0, 1.0, 0.0, 3 * Math.PI / 2, -1)]
        [TestCase(1.0, 1.0, 0.0, 2 * Math.PI, 0)]
        public void SinFunction_Calculate_ReturnsCorrectValues(double amplitude, double frequency, double phase, double input, double expected)
        {
            // Arrange
            _sinFunction = new SinFunction(amplitude, frequency, phase);

            // Act
            double result = _sinFunction.Calculate(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-10));
        }
                

        [Test]
        [TestCase(1.0, 1.0, 0.0, 0.0, 1)]
        [TestCase(1.0, 1.0, 0.0, Math.PI / 2, 0)]
        [TestCase(1.0, 1.0, 0.0, Math.PI, -1)]
        [TestCase(1.0, 1.0, 0.0, 3 * Math.PI / 2, 0)]
        [TestCase(1.0, 1.0, 0.0, 2 * Math.PI, 1)]
        public void CosFunction_Calculate_ReturnsCorrectValues(double amplitude, double frequency, double phase, double input, double expected)
        {
            // Arrange
            _cosFunction = new CosFunction(amplitude, frequency, phase);

            // Act
            double result = _cosFunction.Calculate(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-10));
        }

     
        [Test]
        [TestCase(1.0, 1.0, 0.0, 0.0, 1)]
        [TestCase(1.0, 1.0, 0.0, Math.PI, 0)]
        [TestCase(1.0, 1.0, 0.0, -Math.PI, 0)]
        public void SincFunction_Calculate_ReturnsCorrectValues(double amplitude, double frequency, double phase, double input, double expected)
        {
            // Arrange
            _sincFunction = new SincFunction(amplitude, frequency, phase);

            // Act
            double result = _sincFunction.Calculate(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1e-10));
        }
    }
}
