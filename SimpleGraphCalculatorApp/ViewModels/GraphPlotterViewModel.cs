using OxyPlot.Series;
using OxyPlot;
using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SimpleGraphCalculatorApp.Services;
using SimpleGraphCalculatorApp.Interfaces;
using SimpleGraphCalculatorApp.Commands;
using SimpleGraphCalculatorApp.Models;


namespace SimpleGraphCalculatorApp.ViewModels
{
    public class GraphPlotterViewModel : ViewModelBase
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
        public ObservableCollection<FunctionType> FunctionTypes { get; set; }

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
        
        private GraphPlotterService _graphPlotterService;        
        private FunctionFactory _functionFactory;        
        public readonly IMessageService _messageService;

        public ICommand PlotCommand { get; }
        public ICommand ExportSvgCommand { get; }

        public GraphPlotterViewModel(FunctionFactory functionFactory, IMessageService messageService, FunctionParameters parameters)
        {
            _functionFactory = functionFactory;
            _messageService = messageService;
            
            Parameters = parameters;
            FunctionTypes = new ObservableCollection<FunctionType>((FunctionType[])Enum.GetValues(typeof(FunctionType)));

            PlotCommand = new RelayCommand(execute => PlotFunction());
            ExportSvgCommand = new RelayCommand(execute => ExportSvg());

            PlotFunction(); // Plot on load
        }

        private void PlotFunction()
        {
            try
            {
                _functionFactory = SelectedFunctionType switch
                {
                    FunctionType.Sin => new SinFunctionFactory(),
                    FunctionType.Cos => new CosFunctionFactory(),
                    FunctionType.Sinc => new SincFunctionFactory(),
                    _ => throw new NotSupportedException("Unknown function type.")
                };

                _graphPlotterService = new GraphPlotterService(_functionFactory, Parameters.Amplitude, Parameters.Frequency, Parameters.Phase);


                Graph = new PlotModel { Title = $"{SelectedFunctionType} Function" };
                Graph.Series.Clear();

                var series = new LineSeries();

                // check start and end range and swap if needed
                if (Parameters.RangeStart > Parameters.RangeEnd)
                {                    
                    _messageService.ShowMessage("Range start was greater than range end. Please swap the values!", "Warning");
                    return; 
                }

                // Save Parameters to settings
                SettingsService.Save(Parameters);

                for (double x = Parameters.RangeStart; x <= Parameters.RangeEnd; x += 0.1)
                {
                    series.Points.Add(new DataPoint(x, _graphPlotterService.CalculateFunctionValue(x)));
                }

                Graph.Series.Add(series);
                Graph.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                _messageService.ShowMessage($"An error occurred while plotting the function: {ex.Message}", "Error");
            }
                
        }

        private void ExportSvg()
        {
            var dlg = new SaveFileDialog
            {
                Filter = "SVG files (*.svg)|*.svg",
                FileName = "GraphExport.svg"
            };

            if (dlg.ShowDialog() == true)
            {
                using var stream = File.Create(dlg.FileName);
                var exporter = new SvgExporter { Width = 800, Height = 600 };
                exporter.Export(Graph, stream);
            }
        }
    }
}
