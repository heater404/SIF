using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SIFP.Core.Converters
{
    public class BinningModeEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
                return Binding.DoNothing;

            if (value is BinningModeE binning)
            {
                if (binning == BinningModeE.None
                    || binning == BinningModeE.Analog
                    || binning == BinningModeE.Digital
                    || binning == BinningModeE.Analog_Digital)
                    return true;
                else
                    return false;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
