using SIFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SIFP.Core.Converters
{
    public class SubFrameModeVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((SubFrameModeE)value == SubFrameModeE.Mode_4Phase)
                switch (parameter.ToString())
                {
                    case "Phase1_4":
                        return Visibility.Visible;
                    case "Phase5_8":
                        return Visibility.Collapsed;
                    case "SpecialPhase":
                        return Visibility.Collapsed;
                    default:
                        return Visibility.Collapsed;
                }
            else if ((SubFrameModeE)value == SubFrameModeE.Mode_8Phase)
            {
                switch (parameter.ToString())
                {
                    case "Phase1_4":
                        return Visibility.Visible;
                    case "Phase5_8":
                        return Visibility.Visible;
                    case "SpecialPhase":
                        return Visibility.Collapsed;
                    default:
                        return Visibility.Collapsed;
                }
            }
            else if ((SubFrameModeE)value == SubFrameModeE.Mode_5Phase)
            {
                switch (parameter.ToString())
                {
                    case "Phase1_4":
                        return Visibility.Visible;
                    case "Phase5_8":
                        return Visibility.Collapsed;
                    case "SpecialPhase":
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
            else if ((SubFrameModeE)value == SubFrameModeE.Mode_1SpecialPhase)
            {
                switch (parameter.ToString())
                {
                    case "Phase1_4":
                        return Visibility.Collapsed;
                    case "Phase5_8":
                        return Visibility.Collapsed;
                    case "SpecialPhase":
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
            else
                switch (parameter.ToString())
                {
                    case "Phase1_4":
                        return Visibility.Visible;
                    case "Phase5_8":
                        return Visibility.Visible;
                    case "SpecialPhase":
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
