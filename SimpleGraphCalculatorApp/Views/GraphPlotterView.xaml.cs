using SimpleGraphCalculatorApp.Services;
using SimpleGraphCalculatorApp.ViewModels;
using SimpleGraphCalculatorApp.Interfaces;
using SimpleGraphCalculatorApp.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

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

        private void DoubleOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only digits, decimal point, and minus sign
            char c = e.Text[0];

            if (!char.IsDigit(c) && c != '.' && c != '-')
            {
                e.Handled = true;
                return;
            }

            TextBox textBox = sender as TextBox;

            // Don't allow multiple decimal points
            if (c == '.' && textBox.Text.Contains('.'))
            {
                e.Handled = true;
                return;
            }

            // Don't allow minus sign except at the beginning
            if (c == '-' && textBox.SelectionStart != 0)
            {
                e.Handled = true;
                return;
            }

            // Don't allow multiple minus signs
            if (c == '-' && textBox.Text.Contains('-'))
            {
                e.Handled = true;
                return;
            }
        }

        // Add this new event handler to handle empty TextBox
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
                        
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "1.0";
            }
        }
    }
}
