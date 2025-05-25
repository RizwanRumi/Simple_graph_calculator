using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SimpleGraphCalculatorApp.Converters
{
    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return doubleValue.ToString(culture);
            }
            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value?.ToString();

            // Handle empty or null strings
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return 0.0; // Return default value instead of causing error
            }

            // Try to parse the string
            if (double.TryParse(stringValue, NumberStyles.Float, culture, out double result))
            {
                return result;
            }

            // If parsing fails, return 0 instead of causing error
            return 0.0;
        }
    }
}
