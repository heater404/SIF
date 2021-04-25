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

        public ConfigCorrectionViewModel(IInitArithParams initCorrection, IDialogService dialogService, ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager, eventAggregator)
        {
            this.EventAggregator.GetEvent<ConfigCorrectionRequestEvent>().Subscribe(ConfigCorrection, true);
            this.EventAggregator.GetEvent<ConfigCameraAEChangedEvent>().Subscribe(enable => AE = enable, ThreadOption.BackgroundThread, true);
            this.comm = communication;
            this.dialogService = dialogService;
            corrParams = initCorrection.InitCorrection();
        }

        private async void ConfigCorrection()
        {
            await Task.Run(() =>
            {
                comm.ConfigCorrectionParams(corrParams);
            });
        }

        #region OutPut
        public bool OutPointCloud
        {
            get { return corrParams.OutPutParams.OutPointCloud; }
            set
            {
                corrParams.OutPutParams.OutPointCloud = value;
                RaisePropertyChanged();
                ConfigCorrection();
            }
        }
        public bool OutConfidence
        {
            get { return corrParams.OutPutParams.OutConfidence; }
            set { corrParams.OutPutParams.OutConfidence = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public PointCloudTypeE OutPointCloudType
        {
            get { return corrParams.OutPutParams.OutPointCloudType; }
            set { corrParams.OutPutParams.OutPointCloudType = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public DepthValueTypeE OutDepthValueType
        {
            get { return corrParams.OutPutParams.OutDepthValueType; }
            set { corrParams.OutPutParams.OutDepthValueType = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public DepthDataTypeE OutDepthDataType
        {
            get { return corrParams.OutPutParams.OutDepthDataType; }
            set { corrParams.OutPutParams.OutDepthDataType = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        #endregion

        #region Correction
        public bool CorrBP
        {
            get { return corrParams.CorrParams.CorrBP; }
            set { corrParams.CorrParams.CorrBP = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public bool CorrLen
        {
            get { return corrParams.CorrParams.CorrLen; }
            set { corrParams.CorrParams.CorrLen = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public bool CorrTemp
        {
            get { return corrParams.CorrParams.CorrTemp; }
            set { corrParams.CorrParams.CorrTemp = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public bool CorrOffsetAuto
        {
            get { return corrParams.CorrParams.CorrOffsetAuto; }
            set { corrParams.CorrParams.CorrOffsetAuto = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public bool CorrFPPN
        {
            get { return corrParams.CorrParams.CorrFPPN; }
            set { corrParams.CorrParams.CorrFPPN = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public bool CorrWig
        {
            get { return corrParams.CorrParams.CorrWig; }
            set { corrParams.CorrParams.CorrWig = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public bool CorrFPN
        {
            get { return corrParams.CorrParams.CorrFPN; }
            set { corrParams.CorrParams.CorrWig = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public bool FillInvalidPixels
        {
            get { return corrParams.CorrParams.FillInvalidPixels; }
            set { corrParams.CorrParams.FillInvalidPixels = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public bool CutInvalidPixels
        {
            get { return corrParams.CorrParams.CutInvalidPixels; }
            set { corrParams.CorrParams.CutInvalidPixels = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        public bool CorrOffsetManual
        {
            get { return corrParams.CorrParams.CorrOffsetManual; }
            set { corrParams.CorrParams.CorrOffsetManual = value; RaisePropertyChanged(); ConfigCorrection(); }
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
                ConfigCorrection();
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
                ConfigCorrection();
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
                ConfigCorrection();
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
                ConfigCorrection();
            }
        }
        #endregion

        #region Fusion
        public bool SFDeAliasing
        {
            get { return corrParams.FusionParams.SFDeAliasing; }
            set { corrParams.FusionParams.SFDeAliasing = value; RaisePropertyChanged(); ConfigCorrection(); }
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
                ConfigCorrection();
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
                    ConfigCorrection();
                }
            }
        }
        public bool AntiAliCorr
        {
            get { return corrParams.OthersParams.AntiAliCorr; }
            set { corrParams.OthersParams.AntiAliCorr = value; RaisePropertyChanged(); ConfigCorrection(); }
        }
        #endregion
    }
}
