using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegMap.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class RegMapView : UserControl
    {
        public RegMapView()
        {
            InitializeComponent();
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateColumnsWidth(sender as ListView, 0);
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateColumnsWidth(sender as ListView, 120);
        }

        private void UpdateColumnsWidth(ListView listView, double margin)
        {
            int autoFillColumnIndex = (listView.View as GridView).Columns.Count - 1;
            if (listView.ActualWidth == Double.NaN)
                listView.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            double remainingSpace = listView.ActualWidth;
            for (int i = 0; i < (listView.View as GridView).Columns.Count; i++)
                if (i != autoFillColumnIndex)
                    remainingSpace -= (listView.View as GridView).Columns[i].ActualWidth;
            (listView.View as GridView).Columns[autoFillColumnIndex].Width = remainingSpace >= 0 ? remainingSpace - margin : 0;
        }
    }
}
