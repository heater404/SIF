using ConfigCamera.Views;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Serilog;
using Services.Interfaces;
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

        private string version = "1.03.210517";
        public string Version
        {
            get { return version; }
            set { version = value; RaisePropertyChanged(); }
        }

        private bool isLeftDrawerOpen = false;
        public bool IsLeftDrawerOpen
        {
            get { return isLeftDrawerOpen; }
            set { isLeftDrawerOpen = value; RaisePropertyChanged(); }
        }

        private bool isExpertMode = false;
        public bool IsExpertMode
        {
            get { return isExpertMode; }
            set
            {
                isExpertMode = value;
                RaisePropertyChanged();
            }
        }

        private void SwitchModeCallback(IDialogResult result)
        {
            if (result.Result == ButtonResult.Yes)
            {
                if (comm.SwitchUserAccess(!isExpertMode ? UserAccessType.Expert : UserAccessType.Normal))
                {
                    IsExpertMode = !isExpertMode;
                    if (!IsExpertMode)
                    {
                        this.RegionManager.RequestNavigate(RegionNames.MainRegion, ViewNames.PointCloudView);
                        LeftDrawerContent = container.Resolve(ConfigViewTypes.ConfigCameraView);
                    }
                    this.EventAggregator.GetEvent<UserAccessChangedEvent>().Publish(IsExpertMode ? UserAccessType.Expert : UserAccessType.Normal);
                }
                else
                {
                    this.PrintNoticeLog("SwitchUserAccess Fail,please check the connection status", LogLevel.Error);
                    this.PrintWatchLog("SwitchUserAccess Fail,please check the connection status", LogLevel.Error);
                    IsExpertMode = isExpertMode;
                }
            }
            else
                IsExpertMode = isExpertMode;
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

        private bool isEnable=true;
        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value;RaisePropertyChanged(); }
        }

        private bool isStreaming = false;
        public bool IsStreaming
        {
            get { return isStreaming; }
            set { isStreaming = value;RaisePropertyChanged(); }
        }

        public DelegateCommand OpenPasswordDialogCmd { get; set; }
        public DelegateCommand<Type> OpenLeftDrawerCmd { get; set; }
        public DelegateCommand<string> MainRegionNavigationCmd { get; set; }
        IDialogService dialogService;
        ICommunication comm;
        IContainerExtension container;

        public MainWindowViewModel(ICommunication communication, IContainerExtension container, IDialogService dialogService, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager, eventAggregator)
        {
            this.comm = communication;
            this.dialogService = dialogService;
            this.container = container;
            OpenLeftDrawerCmd = new DelegateCommand<Type>(view =>
              {
                  LeftDrawerContent = container.Resolve(view);
                  IsLeftDrawerOpen = true;
              });

            MainRegionNavigationCmd = new DelegateCommand<string>(view =>
              {
                  regionManager.RequestNavigate(RegionNames.MainRegion, view);
              });

            OpenPasswordDialogCmd = new DelegateCommand(() =>
              {
                  dialogService.ShowDialog(DialogNames.PasswordDialog, SwitchModeCallback);
              });

            LeftDrawerContent = container.Resolve(ConfigViewTypes.ConfigCameraView);
            LeftDrawerContent = container.Resolve(ConfigViewTypes.ConfigCorrectionView);
            LeftDrawerContent = container.Resolve(ConfigViewTypes.ConfigPostProcView);
            LeftDrawerContent = container.Resolve(ConfigViewTypes.ConfigAlgView);

            this.EventAggregator.GetEvent<MainWindowEnableEvent>().Subscribe(b => IsEnable = b, true);

            this.EventAggregator.GetEvent<IsStreamingEvent>().Subscribe(isStreaming => IsStreaming = isStreaming, ThreadOption.BackgroundThread, true);

        }
    }
}
