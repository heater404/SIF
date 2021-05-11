using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using Services.Interfaces;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConfigCorrection.ViewModels
{
    public class ConfigCorrectionViewModel : RegionViewModelBase
    {
        private ICommunication comm;
        private IDialogService dialogService;
        private CorrectionParams corrParams;

        private bool isExpert;
        public bool IsExpert
        {
            get { return isExpert; }
            set { isExpert = value; RaisePropertyChanged(); }
        }
        public ConfigCorrectionViewModel(IInitArithParams initCorrection, IDialogService dialogService, ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager, eventAggregator)
        {
            this.EventAggregator.GetEvent<ConfigCorrectionParamsRequestEvent>().Subscribe(ConfigCorrection, ThreadOption.PublisherThread, true);
            this.EventAggregator.GetEvent<ConfigCameraAEChangedEvent>().Subscribe(enable => AE = enable, ThreadOption.BackgroundThread, true);
            this.EventAggregator.GetEvent<UserAccessChangedEvent>().Subscribe(type =>
            {
                if (type == SIFP.Core.Enums.UserAccessType.Expert)
                    IsExpert = true;
                else
                    IsExpert = false;
            }, true);
            this.comm = communication;
            this.dialogService = dialogService;
            corrParams = initCorrection.InitCorrection();
        }

        private async void ConfigCorrectionAsync()
        {
            await Task.Run(() =>
            {
                comm.ConfigCorrectionParams(corrParams);
            });
        }
        private void ConfigCorrection()
        {
            comm.ConfigCorrectionParams(corrParams);
        }
        #region OutPut
        public bool OutPointCloud
        {
            get { return corrParams.OutPutParams.OutPointCloud; }
            set
            {
                corrParams.OutPutParams.OutPointCloud = value;
                RaisePropertyChanged();
                ConfigCorrectionAsync();
            }
        }
        public bool OutConfidence
        {
            get { return corrParams.OutPutParams.OutConfidence; }
            set { corrParams.OutPutParams.OutConfidence = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public PointCloudTypeE OutPointCloudType
        {
            get { return corrParams.OutPutParams.OutPointCloudType; }
            set { corrParams.OutPutParams.OutPointCloudType = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public DepthValueTypeE OutDepthValueType
        {
            get { return corrParams.OutPutParams.OutDepthValueType; }
            set { corrParams.OutPutParams.OutDepthValueType = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public DepthDataTypeE OutDepthDataType
        {
            get { return corrParams.OutPutParams.OutDepthDataType; }
            set { corrParams.OutPutParams.OutDepthDataType = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        #endregion

        #region Correction
        public bool CorrBP
        {
            get { return corrParams.CorrParams.CorrBP; }
            set { corrParams.CorrParams.CorrBP = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public bool CorrLen
        {
            get { return corrParams.CorrParams.CorrLen; }
            set { corrParams.CorrParams.CorrLen = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public bool CorrTemp
        {
            get { return corrParams.CorrParams.CorrTemp; }
            set { corrParams.CorrParams.CorrTemp = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public bool CorrOffsetAuto
        {
            get { return corrParams.CorrParams.CorrOffsetAuto; }
            set { corrParams.CorrParams.CorrOffsetAuto = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public bool CorrFPPN
        {
            get { return corrParams.CorrParams.CorrFPPN; }
            set { corrParams.CorrParams.CorrFPPN = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public bool CorrWig
        {
            get { return corrParams.CorrParams.CorrWig; }
            set { corrParams.CorrParams.CorrWig = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public bool CorrFPN
        {
            get { return corrParams.CorrParams.CorrFPN; }
            set { corrParams.CorrParams.CorrFPN = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public bool FillInvalidPixels
        {
            get { return corrParams.CorrParams.FillInvalidPixels; }
            set { corrParams.CorrParams.FillInvalidPixels = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public bool CutInvalidPixels
        {
            get { return corrParams.CorrParams.CutInvalidPixels; }
            set { corrParams.CorrParams.CutInvalidPixels = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public bool CorrOffsetManual
        {
            get { return corrParams.CorrParams.CorrOffsetManual; }
            set { corrParams.CorrParams.CorrOffsetManual = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        public Int32 F1CorrOffset
        {
            get { return corrParams.CorrParams.F1CorrOffset; }
            set
            {
                if (value > Int16.MaxValue || value < Int16.MinValue)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{Int16.MinValue},{Int16.MaxValue}]");

                corrParams.CorrParams.F1CorrOffset = value;
                RaisePropertyChanged();
                ConfigCorrectionAsync();
            }
        }
        public Int32 F2CorrOffset
        {
            get { return corrParams.CorrParams.F2CorrOffset; }
            set
            {
                if (value > Int16.MaxValue || value < Int16.MinValue)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{Int16.MinValue},{Int16.MaxValue}]");
                corrParams.CorrParams.F2CorrOffset = value;
                RaisePropertyChanged();
                ConfigCorrectionAsync();
            }
        }
        public Int32 F3CorrOffset
        {
            get { return corrParams.CorrParams.F3CorrOffset; }
            set
            {
                if (value > Int16.MaxValue || value < Int16.MinValue)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{Int16.MinValue},{Int16.MaxValue}]");
                corrParams.CorrParams.F3CorrOffset = value;
                RaisePropertyChanged();
                ConfigCorrectionAsync();
            }
        }
        public Int32 F4CorrOffset
        {
            get { return corrParams.CorrParams.F4CorrOffset; }
            set
            {
                if (value > Int16.MaxValue || value < Int16.MinValue)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{Int16.MinValue},{Int16.MaxValue}]");
                corrParams.CorrParams.F4CorrOffset = value;
                RaisePropertyChanged();
                ConfigCorrectionAsync();
            }
        }
        #endregion

        #region Fusion
        public bool SFDeAliasing
        {
            get { return corrParams.FusionParams.SFDeAliasing; }
            set { corrParams.FusionParams.SFDeAliasing = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }

        public UInt32 PresetMaxDist
        {
            get { return corrParams.FusionParams.PresetMaxDist; }
            set
            {
                if (value > UInt16.MaxValue || value < UInt16.MinValue)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{UInt16.MinValue},{UInt16.MaxValue}]");
                corrParams.FusionParams.PresetMaxDist = value;
                RaisePropertyChanged();
                ConfigCorrectionAsync();
            }
        }
        #endregion

        #region Others
        public bool AE
        {
            get { return corrParams.OthersParams.AE; }
            set
            {
                if (value != corrParams.OthersParams.AE)
                {
                    this.EventAggregator.GetEvent<ConfigCorrectionAEChangedEvent>().Publish(value);
                    corrParams.OthersParams.AE = value;
                    RaisePropertyChanged();
                    ConfigCorrectionAsync();
                }
            }
        }
        public bool AntiAliCorr
        {
            get { return corrParams.OthersParams.AntiAliCorr; }
            set { corrParams.OthersParams.AntiAliCorr = value; RaisePropertyChanged(); ConfigCorrectionAsync(); }
        }
        #endregion
    }
}
