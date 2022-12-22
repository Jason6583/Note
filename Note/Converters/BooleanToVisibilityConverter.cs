using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace Note.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(targetType!=typeof(Visibility))
                throw new NotImplementedException("目标属性必须为Visibility变量");
            if((bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
