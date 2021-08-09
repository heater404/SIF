using Prism.Events;
using Prism.Services.Dialogs;
using Services.Interfaces;
using SIFP.Core;
using SIFP.Core.Mvvm;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SIFP.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IEventAggregator eventAggregator;
        private IDialogService dialogService;
        private ICommunication comm;
        public MainWindow(IEventAggregator eventAggregator, ICommunication comm, IDialogService dialogService)
        {
            InitializeComponent();
            this.eventAggregator = eventAggregator;
            this.comm = comm;
            this.dialogService = dialogService;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            this.eventAggregator.GetEvent<CloseAppEvent>().Publish();
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
            this.eventAggregator.GetEvent<ChangeDrawerRegionSizeEvent>().Publish(e.NewSize);
        }
    }
}
