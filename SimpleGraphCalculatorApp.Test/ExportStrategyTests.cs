using NUnit.Framework;
using OxyPlot.Series;
using OxyPlot;
using SimpleGraphCalculatorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SimpleGraphCalculatorApp.Test
{
    [TestFixture]
    public class ExportStrategyTests
    {
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
            var strategy = new SvgExportStrategy();
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
            var strategy = new XamlExportStrategy();
            var path = "test_output.xaml";

            strategy.Export(model, path);

            Assert.That(File.Exists(path));
            File.Delete(path);
        }

        [Test]
        public void SvgExportStrategy_ShouldThrowException_OnNullModel()
        {
            // Arrange
            var strategy = new SvgExportStrategy();
            string path = "test.svg";

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => strategy.Export(null, path));
        }

        [Test]
        public void SvgExportStrategy_ShouldThrowException_OnInvalidPath()
        {
            // Arrange
            var model = new PlotModel();
            var strategy = new SvgExportStrategy();

            string path = @"Z:\Test\NotExist\output.svg";

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => strategy.Export(model, path));
        }
    }
}
