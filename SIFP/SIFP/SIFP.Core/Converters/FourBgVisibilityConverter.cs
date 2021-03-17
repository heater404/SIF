using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SIFP.Core.Converters
{
    public class FourBgVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue)
                return Binding.DoNothing;

            if ((UInt32)values[0] == 4 && (SubFrameModeE)values[1] == SubFrameModeE.Mode_1SpecialPhase && (SpecialFrameModeE)values[2] == SpecialFrameModeE.Bg)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
