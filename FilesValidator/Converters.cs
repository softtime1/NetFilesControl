using System;
using System.Globalization;
using System.Windows.Data;

namespace FilesValidator.Conveters
{
    public class BoolValidToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valid = (bool)value;
            if (valid)
                return String.Empty;
            return "Pink";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BoolValidToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valid = (bool)value;
            if (valid)
                return "Accept.png";
            return "Decline.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

