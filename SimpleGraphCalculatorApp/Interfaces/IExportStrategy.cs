using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculator.Interfaces
{
    public interface IExportStrategy
    {
        void Export(PlotModel model, string filePath);
    }
}
