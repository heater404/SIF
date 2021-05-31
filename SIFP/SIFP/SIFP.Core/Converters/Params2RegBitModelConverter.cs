using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SIFP.Core.Converters
{
    public class Params2RegBitModelConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (DependencyProperty.UnsetValue == values[0] || DependencyProperty.UnsetValue == values[1])
                return Binding.DoNothing;

            return new { Addr = UInt16.Parse(values[0].ToString()), Index = UInt16.Parse(values[1].ToString()) };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
