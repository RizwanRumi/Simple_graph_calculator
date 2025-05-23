using SimpleGraphCalculatorApp.ViewModels;
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
            //DataContext = new GraphPlotterViewModel();
            DataContext = new GraphPlotterViewModel2();
        }
    }
}
