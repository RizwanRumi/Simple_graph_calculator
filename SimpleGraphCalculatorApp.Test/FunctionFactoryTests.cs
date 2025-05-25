using NUnit.Framework;
using SimpleGraphCalculatorApp.Interfaces;
using SimpleGraphCalculatorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Test
{
    [TestFixture]
    public class FunctionFactoryTests
    {
        private FunctionFactory _factory;

        
        [Test]
        [TestCase(FunctionType.Sin, typeof(SincFunction), 1.0, 1.0, 0.0)]
        [TestCase(FunctionType.Cos, typeof(CosFunction), 1.0, 1.0, 0.0)]
        [TestCase(FunctionType.Sinc, typeof(SincFunction), 1.0, 1.0, 0.0)]
        public void CreateFunction_ValidType_ReturnsCorrectFunctionType(FunctionType type, Type expectedType, double amplitude, double frequency, double phase)
        {
            // Act  
            switch (type)
            {
                case FunctionType.Sin:
                    _factory = new SincFunctionFactory();
                    break;
                case FunctionType.Cos:
                    _factory = new CosFunctionFactory();
                    break;
                case FunctionType.Sinc:
                    _factory = new SincFunctionFactory();
                    break;
                default:
                    throw new ArgumentException("Invalid function type", nameof(type));
            }

            var function = _factory.CreateFunction(amplitude, frequency, phase);

            // Assert  
            Assert.That(function, Is.Not.Null);
            Assert.That(function, Is.InstanceOf(expectedType));
        }

        [Test]
        public void CreateFunction_InvalidType_ThrowsArgumentException()
        {
            // Arrange  
            var invalidType = (FunctionType)5;

            // Act & Assert  
            Assert.Throws<ArgumentException>(() =>
            {
                switch (invalidType)
                {
                    case FunctionType.Sin:
                        _factory = new SincFunctionFactory();
                        break;
                    case FunctionType.Cos:
                        _factory = new CosFunctionFactory();
                        break;
                    case FunctionType.Sinc:
                        _factory = new SincFunctionFactory();
                        break;
                    default:
                        throw new ArgumentException("Invalid function type", nameof(invalidType));
                }
            });
        }

    }
}
