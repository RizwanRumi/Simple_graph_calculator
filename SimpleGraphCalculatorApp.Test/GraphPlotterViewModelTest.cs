using NUnit.Framework;
using SimpleGraphCalculatorApp.ViewModels;
using SimpleGraphCalculatorApp.Models;
using SimpleGraphCalculatorApp.Interfaces;
using SimpleGraphCalculatorApp.Services;
using System.IO;
using System;

namespace SimpleGraphCalculatorApp.Test
{
    [TestFixture]
    public class GraphPlotterViewModelTest
    {
        private FunctionFactory _functionFactory;
        private IMessageService _messageService;
        private FunctionParameters _parameters;

        private GraphPlotterViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _functionFactory = new SinFunctionFactory();
            _messageService = new MessageService();            
            _parameters = new FunctionParameters() { 
                Amplitude = 1.0, 
                Frequency = 1.0, 
                Phase = 0,
                RangeStart = -10.0,
                RangeEnd = 10.0,
            };           
        }

        [Test]
        public void ShouldInitializeWithParameters()
        {
            // Act
            _viewModel = new GraphPlotterViewModel(_functionFactory, _messageService, _parameters);
            
            // Assert
            Assert.AreEqual(1.0, _viewModel.Parameters.Amplitude);
            Assert.AreEqual(FunctionType.Sin, _viewModel.SelectedFunctionType);
        }
       

        [Test]
        public void PlotCommand_ShouldCreateGraphSeries()
        {
            // Arrange 
            _viewModel = new GraphPlotterViewModel(_functionFactory, _messageService, _parameters);
            _viewModel.SelectedFunctionType = FunctionType.Cos;

            // Act
            _viewModel.PlotCommand.Execute(null);

            // Assert
            Assert.That(_viewModel.Graph.Series.Count, Is.GreaterThan(0));
        }

        
        [Test]
        public void PlotCommand_ShouldThrowNullReference_WhenInitializationWithNull()
        {
            // Arrange
            _viewModel = null;
            
            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                _viewModel.PlotCommand.Execute(null); 
            });
        }
    }
}
