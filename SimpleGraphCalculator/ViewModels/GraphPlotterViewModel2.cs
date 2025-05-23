using OxyPlot.Series;
using OxyPlot;
using SimpleGraphCalculatorApp.Commands;
using SimpleGraphCalculatorApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SimpleGraphCalculatorApp.Services;
using SimpleGraphCalculator.Models;
using System.Windows;

namespace SimpleGraphCalculatorApp.ViewModels
{
    public class GraphPlotterViewModel2 : ViewModelBase
    {
        private PlotModel _graph;
        public PlotModel Graph
        {
            get => _graph;
            set 
            {
                _graph = value; 
                OnPropertyChanged(nameof(Graph)); 
            }
        }

        public FunctionParameters Parameters { get; set; }
        
        
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

        private GraphPlotterService GraphPlotterService { get; set; }

        private FunctionFactory FunctionFactory { get; set; }    

        public GraphPlotterViewModel2()
        {            
            Parameters = new FunctionParameters();

            FunctionTypes = new ObservableCollection<FunctionType>((FunctionType[])Enum.GetValues(typeof(FunctionType)));

            PlotCommand = new RelayCommand(execute => PlotFunction());
            
            PlotFunction(); // Plot on load
        }

        private void PlotFunction()
        {
            try
            {
                FunctionFactory = SelectedFunctionType switch
                {
                    FunctionType.Sin => new SinFunctionFactory(),
                    FunctionType.Cos => new CosFunctionFactory(),
                    FunctionType.Sinc => new SincFunctionFactory(),
                    _ => throw new NotSupportedException("Unknown function type.")
                };

                GraphPlotterService = new GraphPlotterService(FunctionFactory, Parameters.Amplitude, Parameters.Frequency, Parameters.Phase);


                Graph = new PlotModel { Title = $"{SelectedFunctionType} Function" };
                Graph.Series.Clear();

                var series = new LineSeries() { Title = Parameters.Type.ToString() };


                // check start and end range and swap if needed
                if (Parameters.RangeStart > Parameters.RangeEnd)
                {
                    (Parameters.RangeEnd, Parameters.RangeStart) = (Parameters.RangeStart, Parameters.RangeEnd);
                }


                for (double x = Parameters.RangeStart; x <= Parameters.RangeEnd; x += 0.1)
                {
                    series.Points.Add(new DataPoint(x, GraphPlotterService.CalculateFunctionValue(x)));
                }

                Graph.Series.Add(series);
                Graph.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while plotting the function: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                
        }
    }
}
