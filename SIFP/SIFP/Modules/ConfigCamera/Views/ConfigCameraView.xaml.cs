using Prism.Events;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ConfigCamera.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class ConfigCameraView : UserControl
    {
        public ConfigCameraView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            eventAggregator.GetEvent<ChangeDrawerRegionSizeEvent>().Subscribe(size=>
            {
                this.Height =Math.Max(size.Height-80,0);
            },true);
        }
    }
}
