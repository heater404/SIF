using SIFP.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace SIFP.Core.Converters
{
    public class EnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is List<RegBitsRange> BitsValidation)
            {
                if (values[1] is UInt16 offset)
                {
                    foreach (var item in BitsValidation)
                    {
                        if (item.Min == item.Max && offset >= item.Start && offset <= item.End)
                            return false;
                    }
                    return true;
                }
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
