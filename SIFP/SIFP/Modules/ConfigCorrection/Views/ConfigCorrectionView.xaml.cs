using Prism.Events;
using SIFP.Core.Mvvm;
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

namespace ConfigCorrection.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class ConfigCorrectionView : UserControl
    {
        IEventAggregator eventAggregator;
        public ConfigCorrectionView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<ChangeDrawerRegionSizeEvent>().Subscribe(size =>
            {
                this.Height = Math.Max(size.Height - 80, 0);
            }, true);
        }

        private void ConfigCorrectionChanged(object sender, RoutedEventArgs e)
        {
            this.eventAggregator?.GetEvent<ConfigCorrectionParamsRequestEvent>().Publish();
        }
    }
}
