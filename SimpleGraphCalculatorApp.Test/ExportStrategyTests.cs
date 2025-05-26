using NUnit.Framework;
using OxyPlot.Series;
using OxyPlot;
using SimpleGraphCalculatorApp.Models;
using System;
using System.IO;
using Moq;
using SimpleGraphCalculatorApp.Interfaces;

namespace SimpleGraphCalculatorApp.Test
{
    [TestFixture]
    public class ExportStrategyTests
    {
        private Mock<IMessageService> mockMessageService;
        
        [SetUp]
        public void SetUp()
        {
            // Create a mock for IMessageService
            mockMessageService = new Mock<IMessageService>();
        }

        private PlotModel CreateSampleModel()
        {
            var model = new PlotModel { Title = "Test Plot" };
            var series = new LineSeries { Title = "Test Series" };
            series.Points.Add(new DataPoint(0, 0));
            series.Points.Add(new DataPoint(1, 1));
            model.Series.Add(series);
            return model;
        }

        [Test]
        public void SvgExportStrategy_ShouldCreateFile()
        {
            // Arrange
            var model = CreateSampleModel();
            var strategy = new SvgExportStrategy(mockMessageService.Object);
            var path = "test_output.svg";

            // Act
            strategy.Export(model, path);

            // Assert
            Assert.That(File.Exists(path));
            File.Delete(path);
        }

        [Test]
        public void XamlExportStrategy_ShouldCreateFile()
        {
            var model = CreateSampleModel();
            var strategy = new XamlExportStrategy(mockMessageService.Object);
            var path = "test_output.xaml";

            strategy.Export(model, path);

            Assert.That(File.Exists(path));
            File.Delete(path);
        }

        [Test]
        public void SvgExportStrategy_ShouldThrowException_OnNullModel()
        {
            // Arrange
            var strategy = new SvgExportStrategy(mockMessageService.Object);
            string path = "test.svg";

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => strategy.Export(null, path));
        }

        [Test]
        public void SvgExportStrategy_ShouldThrowException_OnInvalidPath()
        {
            // Arrange
            var model = new PlotModel();
            var strategy = new SvgExportStrategy(mockMessageService.Object);

            string path = @"Z:\Test\NotExist\output.svg";

            string capturedMessage = null;
            mockMessageService.Setup(m => m.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((msg, title) => capturedMessage = msg);

            // Act & Assert
            strategy.Export(model, path);

            mockMessageService.Verify(m => m.ShowMessage(It.Is<string>(msg => msg.Contains("Error exporting SVG: Directory not found")), "Error"), Times.Once);
            Assert.AreEqual("Error exporting SVG: Directory not found: Z:\\Test\\NotExist", capturedMessage);
        }

        [Test]
        public void XamlExportStrategy_ShouldThrowException_OnInvalidPath()
        {
            // Arrange
            var model = new PlotModel();
            var strategy = new XamlExportStrategy(mockMessageService.Object);

            string path = @"Z:\Test\NotExist\output.xaml";

            string capturedMessage = null;
            mockMessageService.Setup(m => m.ShowMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((msg, title) => capturedMessage = msg);

            // Act & Assert
            strategy.Export(model, path);

            mockMessageService.Verify(m => m.ShowMessage(It.Is<string>(msg => msg.Contains("Error exporting XAML: Directory not found")), "Error"), Times.Once);
            Assert.AreEqual("Error exporting XAML: Directory not found: Z:\\Test\\NotExist", capturedMessage);
        }
    }
}
