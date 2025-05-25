using Moq;
using NUnit.Framework;
using SimpleGraphCalculatorApp.Interfaces;
using SimpleGraphCalculatorApp.Models;
using SimpleGraphCalculatorApp.Services;
using System;

namespace SimpleGraphCalculatorApp.Test
{
    [TestFixture]
    public class GraphPlotterServiceMockTests
    {
        private Mock<FunctionFactory> _mockFactory;
        private Mock<BaseFunction> _mockFunction;
        private GraphPlotterService _service;

        [SetUp]
        public void Setup()
        {
            _mockFactory = new Mock<FunctionFactory>();
            _mockFunction = new Mock<BaseFunction>();
        }

        [Test]
        public void Constructor_WithValidFactory_CreatesService()
        {
            // Arrange
            const double amplitude = 2.0;
            const double frequency = 1.5;
            const double phase = 0.5;

            _mockFactory.Setup(f => f.CreateFunction(amplitude, frequency, phase))
                       .Returns(_mockFunction.Object);

            // Act
            _service = new GraphPlotterService(_mockFactory.Object, amplitude, frequency, phase);

            // Assert
            Assert.That(_service, Is.Not.Null);
            _mockFactory.Verify(f => f.CreateFunction(amplitude, frequency, phase), Times.Once);
        }

        [Test]
        public void CalculateFunctionValue_WithMockedFunction_ReturnsExpectedValue()
        {
            // Arrange
            const double inputX = 1.5;
            const double expectedResult = 0.997;

            _mockFactory.Setup(f => f.CreateFunction(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                       .Returns(_mockFunction.Object);
            _mockFunction.Setup(f => f.Calculate(inputX))
                        .Returns(expectedResult);

            _service = new GraphPlotterService(_mockFactory.Object, 1.0, 1.0, 0.0);

            // Act
            double result = _service.CalculateFunctionValue(inputX);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
            _mockFunction.Verify(f => f.Calculate(inputX), Times.Once);
        }

        [Test]
        public void Constructor_WithMockUsingConstructorArgs_Works()
        {
            // Arrange - Mock BaseFunction with constructor arguments
            _mockFunction = new Mock<BaseFunction>(1.0, 2.0, 3.0); // Pass constructor args

            _mockFactory.Setup(f => f.CreateFunction(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                       .Returns(_mockFunction.Object);

            _mockFunction.Setup(f => f.Calculate(It.IsAny<double>()))
                        .Returns(42.0);

            // Act
            _service = new GraphPlotterService(_mockFactory.Object, 1.0, 2.0, 3.0);
            var result = _service.CalculateFunctionValue(Math.PI);

            // Assert
            Assert.That(result, Is.EqualTo(42.0));
            Assert.That(_mockFunction.Object.Amplitude, Is.EqualTo(1.0));
            Assert.That(_mockFunction.Object.Frequency, Is.EqualTo(2.0));
            Assert.That(_mockFunction.Object.Phase, Is.EqualTo(3.0));
        }
    }
}
