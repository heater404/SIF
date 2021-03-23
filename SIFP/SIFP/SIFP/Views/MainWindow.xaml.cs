using Prism.Events;
using SIFP.Core.Mvvm;
using System;
using System.ComponentModel;
using System.Windows;

namespace SIFP.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IEventAggregator eventAggregator;
        public MainWindow(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.eventAggregator = eventAggregator;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            DrawerHost.OpenMode = MaterialDesignThemes.Wpf.DrawerHostOpenMode.Standard;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            DrawerHost.OpenMode = MaterialDesignThemes.Wpf.DrawerHostOpenMode.Model;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.eventAggregator.GetEvent<ChangeLeftDrawerRegionSizeEvent>().Publish(e.NewSize);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.eventAggregator.GetEvent<DisconnectCameraRequestEvent>().Publish();
            base.OnClosing(e);
        }
    }
}
