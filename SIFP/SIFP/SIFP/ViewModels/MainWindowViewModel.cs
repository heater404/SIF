using ConfigCamera.Views;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Serilog;
using SIFP.Core;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SIFP.ViewModels
{
    public class MainWindowViewModel : RegionViewModelBase
    {
        private string title = "SI View";
        public string Title
        {
            get { return title; }                                                                     
            set { SetProperty(ref title, value); }
        }

        private bool isLeftDrawerOpen=false;
        public bool IsLeftDrawerOpen
        {
            get { return isLeftDrawerOpen; }
            set { isLeftDrawerOpen = value; RaisePropertyChanged(); }
        }

        private bool isDebug;
        public bool IsDebug
        {
            get { return isDebug; }
            set 
            {
                isDebug = value;
                RaisePropertyChanged();
                this.EventAggregator.GetEvent<IsDebugEvent>().Publish(value);
            }
        }

        public DelegateCommand<string> OpenLeftDrawerCmd { get; set; }
        public DelegateCommand<string> MainRegionNavigationCmd { get; set; }
        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            OpenLeftDrawerCmd = new DelegateCommand<string>(view =>
              {
                  regionManager.RequestNavigate(RegionNames.LeftDrawerRegion, view);
                  IsLeftDrawerOpen = true;
              });

            MainRegionNavigationCmd = new DelegateCommand<string>(view =>
              {
                  regionManager.RequestNavigate(RegionNames.MainRegion, view);
              });
        }
    }
}
