using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;

namespace DLUTToolBoxV3.Converters
{
    public class VisibilityBoolenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return Visibility.Collapsed;
            if ((bool)value == true)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
