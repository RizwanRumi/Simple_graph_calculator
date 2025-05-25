using SimpleGraphCalculatorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraphCalculatorApp.Models
{   
    public enum FunctionType 
    { 
        Sin, 
        Cos, 
        Sinc
    }

    public class FunctionParameters : ViewModelBase
    {        
        private double _amplitude = 1.0;
        private double _frequency = 1.0;
        private double _phase = 1.0;
        private double _rangeStart = -10.0;
        private double _rangeEnd = 10.0;
  
        public double Amplitude
        {
            get => _amplitude;
            set
            {
                _amplitude = value;
                OnPropertyChanged(nameof(Amplitude));
            }
        }
        
        public double Frequency
        {
            get => _frequency;
            set
            {
                _frequency = value;
                OnPropertyChanged(nameof(Frequency));
            }
        }

        public double Phase
        {
            get => _phase;
            set
            {
                _phase = value;
                OnPropertyChanged(nameof(Phase));
            }
        }
        
        public double RangeStart
        {
            get => _rangeStart;
            set
            {
                _rangeStart = value;
                OnPropertyChanged(nameof(RangeStart));
            }
        }

        public double RangeEnd
        {
            get => _rangeEnd;
            set
            {
                _rangeEnd = value;
                OnPropertyChanged(nameof(RangeEnd));
            }
        }
    }
}
