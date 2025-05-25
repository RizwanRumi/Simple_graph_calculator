using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using SimpleGraphCalculator.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Markup;

namespace SimpleGraphCalculatorApp.Models
{
    public enum VectorExportFormat
    {
        SVG,
        XAML
    }

    public static class VectorExportFormatValues
    {
        public static IEnumerable<VectorExportFormat> Values =>
            (VectorExportFormat[])Enum.GetValues(typeof(VectorExportFormat));
    }

    public class SvgExportStrategy : IExportStrategy
    {
        public void Export(PlotModel model, string filePath)
        {
            using var stream = File.Create(filePath);
            var exporter = new OxyPlot.SvgExporter { Width = 800, Height = 600 };
            exporter.Export(model, stream);
        }
    }

    public class XamlExportStrategy : IExportStrategy
    {
        public void Export(PlotModel model, string filePath)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            sb.AppendLine("        xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">");

            foreach (var series in model.Series)
            {
                if (series is LineSeries lineSeries)
                {
                    var pathData = new StringBuilder("M ");

                    foreach (var pt in lineSeries.Points)
                    {
                        // Scale X/Y for display (optional: you can map real-world to pixels)
                        double x = pt.X * 20 + 100;
                        double y = 300 - pt.Y * 20;

                        pathData.AppendFormat("{0},{1} ", x, y);
                    }

                    sb.AppendLine($@"<Path Stroke=""Blue"" StrokeThickness=""2"">
                                        <Path.Data>
                                          <PathGeometry Figures=""{pathData.ToString().Trim()}""/>
                                        </Path.Data>
                                      </Path>");
                }
            }

            sb.AppendLine("</Canvas>");

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
