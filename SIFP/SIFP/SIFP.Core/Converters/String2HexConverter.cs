using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace SIFP.Core.Converters
{
    public class String2HexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UInt32 res = 0;
            try
            {
                res = System.Convert.ToUInt32(value.ToString(), 16);
            }
            catch (Exception)
            {
                Console.WriteLine(value.ToString() + "is not a hex");
            }
            return res;
        }
    }
}
