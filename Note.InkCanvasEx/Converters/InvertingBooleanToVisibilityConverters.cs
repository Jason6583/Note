using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Note.InkCanvasEx.Converters
{
    public class InvertingBooleanToVisibilityConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("目标属性必须为Visibility类型");
            if (!(bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
