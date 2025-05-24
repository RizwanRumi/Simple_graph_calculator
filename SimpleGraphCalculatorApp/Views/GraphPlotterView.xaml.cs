using SimpleGraphCalculatorApp.Services;
using SimpleGraphCalculatorApp.ViewModels;
using SimpleGraphCalculatorApp.Interfaces;
using SimpleGraphCalculatorApp.Models;
using System.Windows;

namespace SimpleGraphCalculatorApp.Views
{
    /// <summary>
    /// Interaction logic for GraphPlotterView.xaml
    /// </summary>
    public partial class GraphPlotterView : Window
    {        
        public GraphPlotterView()
        {
            InitializeComponent();

            IMessageService messageService = new MessageService();
            FunctionFactory functionFactory = new SinFunctionFactory();
            FunctionParameters parameters = SettingsService.Load(); // Load on startup;

            DataContext = new GraphPlotterViewModel(functionFactory, messageService, parameters);
        }
    }
}
