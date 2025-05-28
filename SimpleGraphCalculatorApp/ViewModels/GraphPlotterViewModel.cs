using OxyPlot.Series;
using OxyPlot;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SimpleGraphCalculatorApp.Services;
using SimpleGraphCalculatorApp.Interfaces;
using SimpleGraphCalculatorApp.Commands;
using SimpleGraphCalculatorApp.Models;
using SimpleGraphCalculator.Interfaces;
using System.IO;



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
        public ICommand ExportCommand { get; }

        public VectorExportFormat SelectedExportFormat { get; set; } = VectorExportFormat.SVG;
        

        public GraphPlotterViewModel(FunctionFactory functionFactory, IMessageService messageService, FunctionParameters parameters)
        {
            _functionFactory = functionFactory;
            _messageService = messageService;
            
            Parameters = parameters;
            FunctionTypes = new ObservableCollection<FunctionType>((FunctionType[])Enum.GetValues(typeof(FunctionType)));

            PlotCommand = new RelayCommand(execute => PlotFunction());           
            ExportCommand = new RelayCommand(execute => ExportGraph());

            if(File.Exists(SettingsService.FilePath))
            {
                PlotFunction(); // Plot on load
            }            
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

                // check start and end range                
                if (CheckValidRanges(Parameters.RangeStart, Parameters.RangeEnd))
                {                   
                    // SaveParameters Parameters to settings
                    SettingsService.SaveParameters(Parameters);

                    for (double x = Parameters.RangeStart; x <= Parameters.RangeEnd; x += 0.1)
                    {
                        series.Points.Add(new DataPoint(x, _graphPlotterService.CalculateFunctionValue(x)));
                    }

                    Graph.Series.Add(series);
                    Graph.InvalidatePlot(true);
                }
                else
                {
                    return;
                }


            }
            catch (Exception ex)
            {
                _messageService.ShowMessage($"An error occurred while plotting the function: {ex.Message}", "Error");
            }
                
        }

        private bool CheckValidRanges(double rangeStart, double rangeEnd)
        {
            if (Parameters.RangeStart > Parameters.RangeEnd)
            {
                _messageService.ShowMessage("Range start was greater than range end. Please swap the values!", "Warning");
                 return false;
            }
            else if (Parameters.RangeStart == Parameters.RangeEnd)
            {
                _messageService.ShowMessage("Range start and end are equal. Please change the values!", "Warning");
                return false;
            }

            return true;
        }

        private void ExportGraph()
        {
            var dlg = new SaveFileDialog();

            IExportStrategy strategy;

            switch (SelectedExportFormat)
            {
                case VectorExportFormat.SVG:
                    dlg.Filter = "SVG files (*.svg)|*.svg";
                    dlg.FileName = "GraphExport.svg";
                    strategy = new SvgExportStrategy(_messageService);
                    break;

                case VectorExportFormat.XAML:
                    dlg.Filter = "XAML files (*.xaml)|*.xaml";
                    dlg.FileName = "GraphExport.xaml";
                    strategy = new XamlExportStrategy(_messageService);
                    break;

                default:
                    _messageService.ShowMessage("Unsupported export format.");
                    return;
            }

            if (dlg.ShowDialog() == true)
            {
                var exporter = new GraphExporterService(strategy);
                exporter.Export(Graph, dlg.FileName);
            }
        }
    }
}
