using OxyPlot;
using SimpleGraphCalculator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Services
{

    public class GraphExporterService
    {
        private readonly IExportStrategy _strategy;

        public GraphExporterService(IExportStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Export(PlotModel model, string filePath)
        {
            _strategy.Export(model, filePath);
        }
    }
}
