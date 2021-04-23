using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Services.Interfaces;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;

namespace ConfigCorrection.ViewModels
{
    public class ConfigCorrectionViewModel : RegionViewModelBase
    {
        const int MaxCorrOffset = Int16.MaxValue;
        const int MinCorrOffset = Int16.MinValue;
        const uint MaxPresetDis = UInt16.MaxValue;
        const uint MinPresetDis = UInt16.MinValue;
        private ICommunication comm;
        private IDialogService dialogService;
        private CorrectionParameters corrParams = new CorrectionParameters();

        public ConfigCorrectionViewModel(IDialogService dialogService, ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager, eventAggregator)
        {

        }

        #region OutPut
        public Array PointCloudTypes { get => Enum.GetValues(typeof(PointCloudTypeE)); }
        public Array DepthValueTypes { get => Enum.GetValues(typeof(DepthValueTypeE)); }
        public Array DepthDataTypes { get => Enum.GetValues(typeof(DepthDataTypeE)); }
        public bool OutPointCloud
        {
            get { return corrParams.OutPutParams.OutPointCloud; }
            set { corrParams.OutPutParams.OutPointCloud = value; RaisePropertyChanged(); }
        }
        public bool OutConfidence
        {
            get { return corrParams.OutPutParams.OutConfidence; }
            set { corrParams.OutPutParams.OutConfidence = value; RaisePropertyChanged(); }
        }
        public PointCloudTypeE OutPointCloudType
        {
            get { return corrParams.OutPutParams.OutPointCloudType; }
            set { corrParams.OutPutParams.OutPointCloudType = value; RaisePropertyChanged(); }
        }
        public DepthValueTypeE OutDepthValueType
        {
            get { return corrParams.OutPutParams.OutDepthValueType; }
            set { corrParams.OutPutParams.OutDepthValueType = value; RaisePropertyChanged(); }
        }
        public DepthDataTypeE OutDepthDataType
        {
            get { return corrParams.OutPutParams.OutDepthDataType; }
            set { corrParams.OutPutParams.OutDepthDataType = value; RaisePropertyChanged(); }
        }
        #endregion

        #region Correction
        public bool CorrBP
        {
            get { return corrParams.CorrectionParams.CorrBP; }
            set { corrParams.CorrectionParams.CorrBP = value; RaisePropertyChanged(); }
        }
        public bool CorrLen
        {
            get { return corrParams.CorrectionParams.CorrLen; }
            set { corrParams.CorrectionParams.CorrLen = value; RaisePropertyChanged(); }
        }
        public bool CorrTemp
        {
            get { return corrParams.CorrectionParams.CorrTemp; }
            set { corrParams.CorrectionParams.CorrTemp = value; RaisePropertyChanged(); }
        }
        public bool CorrOffsetAuto
        {
            get { return corrParams.CorrectionParams.CorrOffsetAuto; }
            set { corrParams.CorrectionParams.CorrOffsetAuto = value; RaisePropertyChanged(); }
        }
        public bool CorrFPPN
        {
            get { return corrParams.CorrectionParams.CorrFPPN; }
            set { corrParams.CorrectionParams.CorrFPPN = value; RaisePropertyChanged(); }
        }
        public bool CorrWig
        {
            get { return corrParams.CorrectionParams.CorrWig; }
            set { corrParams.CorrectionParams.CorrWig = value; RaisePropertyChanged(); }
        }
        public bool CorrFPN
        {
            get { return corrParams.CorrectionParams.CorrFPN; }
            set { corrParams.CorrectionParams.CorrWig = value; RaisePropertyChanged(); }
        }
        public bool FillInvalidPixels
        {
            get { return corrParams.CorrectionParams.FillInvalidPixels; }
            set { corrParams.CorrectionParams.FillInvalidPixels = value; RaisePropertyChanged(); }
        }
        public bool CutInvalidPixels
        {
            get { return corrParams.CorrectionParams.CutInvalidPixels; }
            set { corrParams.CorrectionParams.CutInvalidPixels = value; RaisePropertyChanged(); }
        }
        public bool CorrOffsetManual
        {
            get { return corrParams.CorrectionParams.CorrOffsetManual; }
            set { corrParams.CorrectionParams.CorrOffsetManual = value; RaisePropertyChanged(); }
        }
        public Int32 F1CorrOffset
        {
            get { return corrParams.CorrectionParams.F1CorrOffset; }
            set
            {
                if (value > MaxCorrOffset || value < MinCorrOffset)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{MinCorrOffset},{MaxCorrOffset}]");

                corrParams.CorrectionParams.F1CorrOffset = value;
                RaisePropertyChanged();
            }
        }
        public Int32 F2CorrOffset
        {
            get { return corrParams.CorrectionParams.F2CorrOffset; }
            set
            {
                if (value > MaxCorrOffset || value < MinCorrOffset)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{MinCorrOffset},{MaxCorrOffset}]");
                corrParams.CorrectionParams.F2CorrOffset = value;
                RaisePropertyChanged();
            }
        }
        public Int32 F3CorrOffset
        {
            get { return corrParams.CorrectionParams.F3CorrOffset; }
            set
            {
                if (value > MaxCorrOffset || value < MinCorrOffset)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{MinCorrOffset},{MaxCorrOffset}]");
                corrParams.CorrectionParams.F3CorrOffset = value;
                RaisePropertyChanged();
            }
        }
        public Int32 F4CorrOffset
        {
            get { return corrParams.CorrectionParams.F4CorrOffset; }
            set
            {
                if (value > MaxCorrOffset || value < MinCorrOffset)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{MinCorrOffset},{MaxCorrOffset}]");
                corrParams.CorrectionParams.F4CorrOffset = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Fusion
        public bool SFDeAliasing
        {
            get { return corrParams.FusionParams.SFDeAliasing; }
            set { corrParams.FusionParams.SFDeAliasing = value;RaisePropertyChanged(); }
        }

        public UInt32 PresetMaxDist
        {
            get { return corrParams.FusionParams.PresetMaxDist; }
            set 
            {
                if (value > MaxPresetDis || value < MinPresetDis)
                    throw new ArgumentOutOfRangeException($"OutOfRange:[{MinPresetDis},{MaxPresetDis}]");
                corrParams.FusionParams.PresetMaxDist = value; 
                RaisePropertyChanged(); 
            }
        }
        #endregion

        #region Others
        public bool AE
        {
            get { return corrParams.OthersParams.AE; }
            set { corrParams.OthersParams.AE = value; RaisePropertyChanged(); }
        }
        public bool AntiAliCorr
        {
            get { return corrParams.OthersParams.AntiAliCorr; }
            set { corrParams.OthersParams.AntiAliCorr = value; RaisePropertyChanged(); }
        }
        #endregion
    }
}
