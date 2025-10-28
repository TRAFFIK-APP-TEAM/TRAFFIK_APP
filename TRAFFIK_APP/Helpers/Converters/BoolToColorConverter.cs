using System;
using System.Globalization;

namespace TRAFFIK_APP.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isUsed)
            {
                return isUsed ? "#28a745" : "#ffc107"; // Green for used, Yellow/Orange for active
            }
            return "#666";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

