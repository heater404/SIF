using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace SIFP.Core.Converters
{
    public class IntegrationTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)((UInt32)value / 1000);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (UInt32)((double.Parse(value.ToString()) * 1000));
        }
    }
}
