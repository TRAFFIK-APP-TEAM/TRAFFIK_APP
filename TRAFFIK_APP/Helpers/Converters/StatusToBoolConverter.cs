using System;
using System.Globalization;

namespace TRAFFIK_APP.Converters
{
    public class StatusToBoolConverter : Microsoft.Maui.Controls.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status && parameter is string targetStatus)
            {
                return status.Equals(targetStatus, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

