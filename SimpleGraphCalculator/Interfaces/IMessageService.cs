using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculator.Interfaces
{
    public interface IMessageService
    {
        void ShowMessage(string message);
        void ShowMessage(string message, string messgeType);
        bool ShowConfirmation(string message, string confirmation, string option);        
    }
}
