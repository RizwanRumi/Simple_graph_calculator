using SimpleGraphCalculatorApp.Interfaces;
using System.Windows;

namespace SimpleGraphCalculatorApp.Services
{
    public class MessageService : IMessageService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        public void ShowMessage(string message, string messgeType)
        {
            if (messgeType == "Error")
                MessageBox.Show(message, messgeType, MessageBoxButton.OK, MessageBoxImage.Error);
            else if (messgeType == "Warning")
                MessageBox.Show(message, messgeType, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public bool ShowConfirmation(string message, string confirmation, string option)
        {
            if (option == "Question")
                return MessageBox.Show(message, confirmation, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
            else if (option == "Warning")
                return MessageBox.Show(message, confirmation, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
            else
                return false;
        }
    }
}
