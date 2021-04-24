using ConfigCamera.Views;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
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

        private bool isLeftDrawerOpen = false;
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

        private object leftDrawerContent;
        public object LeftDrawerContent
        {
            get { return leftDrawerContent; }
            set { leftDrawerContent = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<Type> OpenLeftDrawerCmd { get; set; }
        public DelegateCommand<string> MainRegionNavigationCmd { get; set; }
        public MainWindowViewModel(IContainerExtension container, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            OpenLeftDrawerCmd = new DelegateCommand<Type>(view =>
              {
                  LeftDrawerContent = container.Resolve(view);
                  IsLeftDrawerOpen = true;
              });

            MainRegionNavigationCmd = new DelegateCommand<string>(view =>
              {
                  regionManager.RequestNavigate(RegionNames.MainRegion, view);
              });

            LeftDrawerContent= container.Resolve(ConfigViewTypes.ConfigCameraView);
            LeftDrawerContent = container.Resolve(ConfigViewTypes.ConfigCorrectionView);
            LeftDrawerContent = container.Resolve(ConfigViewTypes.ConfigPostProcView);
        }
    }
}
