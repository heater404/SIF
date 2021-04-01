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

namespace PointCloud.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class PointCloudView : UserControl
    {
        
        public PointCloudView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            eventAggregator.GetEvent<OpenPointCloudEvent>().Subscribe(OpenPointCloud,true);
            eventAggregator.GetEvent<ClosePointCloudEvent>().Subscribe(ClosePointCloud,true);
        }

        private void ClosePointCloud()
        {
            PointCloud.CloseProcess();
        }

        private void OpenPointCloud(string args)
        {
            PointCloud.StartAndEmbedProcess(@"View3D\PowerPixel3D.exe", args);
        }
    }
}
