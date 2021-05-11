using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Services.Interfaces;
using SIFP.Core;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConifgAlg.ViewModels
{
    public class ConfigAlgViewModel : RegionViewModelBase
    {
        ICommunication comm;
        IDialogService dialogService;
        private ConfigAlg config;
        public ConfigAlgViewModel(IDialogService dialogService, IInitConfigAlg initConfig, ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            this.comm = communication;
            this.dialogService = dialogService;
            config = initConfig.Init();
            this.ApplyConfigAlgCmd = new DelegateCommand(ApplyConfigAlgAsync);
            this.EventAggregator.GetEvent<ConfigAlgRequestEvent>().Subscribe(ApplyConfigAlg, ThreadOption.PublisherThread, true);
            this.EventAggregator.GetEvent<IsStreamingEvent>().Subscribe(isStreaming => IsEnable = !isStreaming, ThreadOption.BackgroundThread, true);
        }

        private async void ApplyConfigAlgAsync()
        {
            dialogService.Show(DialogNames.WaitingDialog);
            var res = await Task.Run(() => comm.ConfigAlg(new ConfigAlgRequest { Config = config }, 3000));
            if (res.HasValue)
            {
                if (res.Value)
                {
                    this.PrintNoticeLog("ConfigAlg Success", LogLevel.Warning);
                    this.PrintWatchLog("ConfigAlg Success", LogLevel.Warning);
                }
                else
                {
                    this.PrintNoticeLog("ConfigAlg Fail", LogLevel.Error);
                    this.PrintWatchLog("ConfigAlg Fail", LogLevel.Error);
                }
            }
            else
            {
                this.PrintNoticeLog("ConfigAlg Timeout", LogLevel.Error);
                this.PrintWatchLog("ConfigAlg Timeout", LogLevel.Error);
            }
            EventAggregator.GetEvent<CloseWaitingDialogEvent>().Publish();
        }
        private void ApplyConfigAlg()
        {
            var res =  comm.ConfigAlg(new ConfigAlgRequest { Config = config }, 3000);
            if (res.HasValue)
            {
                if (res.Value)
                {
                    this.PrintNoticeLog("ConfigAlg Success", LogLevel.Warning);
                    this.PrintWatchLog("ConfigAlg Success", LogLevel.Warning);
                }
                else
                {
                    this.PrintNoticeLog("ConfigAlg Fail", LogLevel.Error);
                    this.PrintWatchLog("ConfigAlg Fail", LogLevel.Error);
                }
            }
            else
            {
                this.PrintNoticeLog("ConfigAlg Timeout", LogLevel.Error);
                this.PrintWatchLog("ConfigAlg Timeout", LogLevel.Error);
            }
        }

        public DelegateCommand ApplyConfigAlgCmd { get; set; }

        private bool isEnable = true;
        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; RaisePropertyChanged(); }
        }

        public bool ByPassSocAlgorithm
        {
            get { return config.ByPassSocAlgorithm; }
            set { config.ByPassSocAlgorithm = value; RaisePropertyChanged(); }
        }
        public bool ReturnRawData
        {
            get { return config.ReturnRawData; }
            set { config.ReturnRawData = value; RaisePropertyChanged(); }
        }
        public bool ReturnDepthIamge
        {
            get { return config.ReturnDepthIamge; }
            set { config.ReturnDepthIamge = value; RaisePropertyChanged(); }
        }
        public bool ReturnGrayImage
        {
            get { return config.ReturnGrayImage; }
            set { config.ReturnGrayImage = value; RaisePropertyChanged(); }
        }
        public bool ReturnBGImage
        {
            get { return config.ReturnBGImage; }
            set { config.ReturnBGImage = value; RaisePropertyChanged(); }
        }
        public bool ReturnAmplitudeImage
        {
            get { return config.ReturnAmplitudeImage; }
            set { config.ReturnAmplitudeImage = value; RaisePropertyChanged(); }
        }
        public bool ReturnConfidence
        {
            get { return config.ReturnConfidence; }
            set { config.ReturnConfidence = value; RaisePropertyChanged(); }
        }
        public bool ReturnFlagMap
        {
            get { return config.ReturnFlagMap; }
            set { config.ReturnFlagMap = value; RaisePropertyChanged(); }
        }
        public bool ReturnPointcloud
        {
            get { return config.ReturnPointcloud; }
            set { config.ReturnPointcloud = value; RaisePropertyChanged(); }
        }
    }
}
