using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace SIFP.Core.Converters
{
    public class FrequencyConverter : IValueConverter
    {
        //value应该是一个KHz的无符整型，需要把它转换为一个Mhz的保留两位小数的浮点型的字符串
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is UInt32 freKHz)
            {
                return (freKHz / 1000.0).ToString("0.00");
            }
            return Binding.DoNothing;
        }

        //将一个Mhz的保留两位小数的浮点型的字符串  转换为一个KHz的无符整型
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            double freMhz = double.Parse(value.ToString());

            return (UInt32)(freMhz * 1000);
        }
    }
}
