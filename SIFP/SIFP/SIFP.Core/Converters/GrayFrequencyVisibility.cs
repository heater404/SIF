using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SIFP.Core.Converters
{
    public class GrayFrequencyVisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                return Binding.DoNothing;

            SpecialFrameModeE sfmode = (SpecialFrameModeE)values[0];

            if (sfmode == SpecialFrameModeE.Gray && (bool)values[1])
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
