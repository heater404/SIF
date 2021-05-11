using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Services.Interfaces;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigPostProc.ViewModels
{
    public class ConfigPostProcViewModel : RegionViewModelBase
    {
        private ICommunication comm;
        private IDialogService dialogService;
        private PostProcParams postProcParams;
        private bool isExpert;
        public bool IsExpert
        {
            get { return isExpert; }
            set { isExpert = value; RaisePropertyChanged(); }
        }
        public ConfigPostProcViewModel(IInitArithParams initArithParams, IDialogService dialogService, ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager, eventAggregator)
        {
            this.EventAggregator.GetEvent<ConfigPostProcParamsRequestEvent>().Subscribe(ConfigPostProc, ThreadOption.PublisherThread, true);
            this.EventAggregator.GetEvent<UserAccessChangedEvent>().Subscribe(type =>
            {
                if (type == SIFP.Core.Enums.UserAccessType.Expert)
                    IsExpert = true;
                else
                    IsExpert = false;
            }, true);
            this.comm = communication;
            this.dialogService = dialogService;
            this.postProcParams = initArithParams.InitPostProc();
        }

        private async void ConfigPostProcAsync()
        {
            await Task.Run(() =>
            {
                comm.ConfigPostProcParams(postProcParams);
            });
        }

        private void ConfigPostProc()
        {
            comm.ConfigPostProcParams(postProcParams);
        }

        #region PostProcOutPut
        public bool OutPointCloud
        {
            get { return postProcParams.OutPutParams.OutPointCloud; }
            set { postProcParams.OutPutParams.OutPointCloud = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        public bool OutConfidence
        {
            get { return postProcParams.OutPutParams.OutConfidence; }
            set { postProcParams.OutPutParams.OutConfidence = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        public bool OutFlag
        {
            get { return postProcParams.OutPutParams.OutFlag; }
            set { postProcParams.OutPutParams.OutFlag = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        public PointCloudTypeE OutPointCloudType
        {
            get { return postProcParams.OutPutParams.OutPointCloudType; }
            set { postProcParams.OutPutParams.OutPointCloudType = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        public DepthValueTypeE OutDepthValueType
        {
            get { return postProcParams.OutPutParams.OutDepthValueType; }
            set { postProcParams.OutPutParams.OutDepthValueType = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        public DepthDataTypeE OutDepthDataType
        {
            get { return postProcParams.OutPutParams.OutDepthDataType; }
            set { postProcParams.OutPutParams.OutDepthDataType = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        #endregion

        #region Denoising
        public DenoiseLevelE TDenoiseLevel
        {
            get { return postProcParams.DenoisingParams.TDenoiseLevel; }
            set { postProcParams.DenoisingParams.TDenoiseLevel = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        public DenoiseLevelE SDenoiseLevel
        {
            get { return postProcParams.DenoisingParams.SDenoiseLevel; }
            set { postProcParams.DenoisingParams.SDenoiseLevel = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        public SDenoiseMethodE SDenoiseMethod
        {
            get { return postProcParams.DenoisingParams.SDenoiseMethod; }
            set { postProcParams.DenoisingParams.SDenoiseMethod = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        #endregion

        #region Repair
        public bool DeFlyPixel
        {
            get { return postProcParams.RepairParams.DeFlyPixel; }
            set { postProcParams.RepairParams.DeFlyPixel = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        public bool DeHoles
        {
            get { return postProcParams.RepairParams.DeHoles; }
            set { postProcParams.RepairParams.DeHoles = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        #endregion

        #region AntiInterference
        public bool AntiALI
        {
            get { return postProcParams.AntiInterferenceParams.AntiALI; }
            set { postProcParams.AntiInterferenceParams.AntiALI = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        public bool AntiMCI
        {
            get { return postProcParams.AntiInterferenceParams.AntiMCI; }
            set { postProcParams.AntiInterferenceParams.AntiMCI = value; RaisePropertyChanged(); ConfigPostProcAsync(); }
        }
        #endregion

        #region Confidence
        public UInt32 ValidDistMin
        {
            get { return postProcParams.ConfidenceParams.ValidDistMin; }
            set
            {
                if (value > UInt16.MaxValue || value < UInt16.MinValue)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{UInt16.MinValue},{UInt16.MaxValue}]");

                if (value > ValidDistMax)
                    throw new ArgumentException($"{value} is greater than ValidDistMax");

                postProcParams.ConfidenceParams.ValidDistMin = value;
                RaisePropertyChanged();
                ConfigPostProcAsync();
            }
        }
        public UInt32 ValidDistMax
        {
            get { return postProcParams.ConfidenceParams.ValidDistMax; }
            set
            {
                if (value > UInt16.MaxValue || value < UInt16.MinValue)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{UInt16.MinValue},{UInt16.MaxValue}]");

                if (value < ValidDistMin)
                    throw new ArgumentException($"{value} is less than ValidDistMin");

                postProcParams.ConfidenceParams.ValidDistMax = value;
                RaisePropertyChanged();
                ConfigPostProcAsync();
            }
        }
        #endregion
    }
}
