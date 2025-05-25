using NUnit.Framework;
using SimpleGraphCalculatorApp.Interfaces;
using SimpleGraphCalculatorApp.Models;
using SimpleGraphCalculatorApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Test
{
    [TestFixture]
    public class GraphPlotterServiceTest
    {
        private IFunction _currentFunction;
        private GraphPlotterService _service;
        private FunctionFactory _factory;

        
        [Test]
        public void GraphPlotterService_SetSinFunction_WorksCorrectly()
        {
            // Arrange
            _factory = new SinFunctionFactory();
            _service = new GraphPlotterService(_factory, 1.0, 1.0, 0.0);

            // Act & Assert
            Assert.DoesNotThrow(() => _service.CalculateFunctionValue(0));
        }
       
        [Test]
        public void GraphPlotterService_SetCosFunction_WorksCorrectly()
        {
            // Arrange
            _factory = new CosFunctionFactory();
            _service = new GraphPlotterService(_factory, 1.0, 1.0, 0.0);

            // Act & Assert
            Assert.DoesNotThrow(() => _service.CalculateFunctionValue(1));
        }

        [Test]
        public void GraphPlotterService_SetSincFunction_WorksCorrectly()
        {
            // Arrange
            _factory = new SincFunctionFactory();
            _service = new GraphPlotterService(_factory, 1.0, 1.0, 0.0);

            // Act & Assert
            Assert.DoesNotThrow(() => _service.CalculateFunctionValue(2));
        }


        [Test]
        public void GraphPlotterService_Functionfactory_DoesNotWorksCorrectly()
        {
            // Arrange
            _factory = null;            

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
            { 
                _service = new GraphPlotterService(_factory, 1.0, 1.0, 0.0); 
            });
        }       

    }
}
