using OxyPlot;
using OxyPlot.Series;
using SimpleGraphCalculatorApp.Commands;
using SimpleGraphCalculatorApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleGraphCalculatorApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public FunctionParameters Parameters { get; set; } = new();
        public PlotModel Graph { get; set; } = new PlotModel { Title = "Function Plot" };


        private FunctionType _selectedFunctionType;
        public FunctionType SelectedFunctionType
        {
            get { return _selectedFunctionType; }
            set
            {
                if (_selectedFunctionType != value)
                {
                    _selectedFunctionType = value;
                    OnPropertyChanged(nameof(SelectedFunctionType));
                }
            }
        }

        public ObservableCollection<FunctionType> FunctionTypes { get; set; }



        public ICommand PlotCommand { get; }

        public MainWindowViewModel()
        {
            FunctionTypes = new ObservableCollection<FunctionType>((FunctionType[])Enum.GetValues(typeof(FunctionType)));

            PlotCommand = new RelayCommand(execute => PlotFunction());
            PlotFunction(); // Plot on load
        }

        private void PlotFunction()
        {
            Graph.Series.Clear();
            var series = new LineSeries { Title = Parameters.Type.ToString() };

            for (double x = Parameters.RangeStart; x <= Parameters.RangeEnd; x += 0.1)
            {
                double y = 0;
                double a = Parameters.Amplitude;
                double f = Parameters.Frequency;
                double p = Parameters.Phase;

                switch (SelectedFunctionType)
                {
                    case FunctionType.Sin:
                        y = a * Math.Sin(f * x + p);
                        break;
                    case FunctionType.Cos:
                        y = a * Math.Cos(f * x + p);
                        break;
                    case FunctionType.Sinc:
                        y = x == 0 ? a : a * Math.Sin(f * x + p) / (f * x + p);
                        break;
                }
                series.Points.Add(new DataPoint(x, y));
            }

            Graph.Series.Add(series);
            Graph.InvalidatePlot(true);
            OnPropertyChanged(nameof(Graph));
        }
    }
}
