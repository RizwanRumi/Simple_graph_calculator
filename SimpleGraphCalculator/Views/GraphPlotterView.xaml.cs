using SimpleGraphCalculatorApp.ViewModels;
using System.Windows;

namespace SimpleGraphCalculator.Views
{
    /// <summary>
    /// Interaction logic for GraphPlotterView.xaml
    /// </summary>
    public partial class GraphPlotterView : Window
    {
        public GraphPlotterView()
        {
            InitializeComponent();
            DataContext = new GraphPlotterViewModel();
        }
    }
}
