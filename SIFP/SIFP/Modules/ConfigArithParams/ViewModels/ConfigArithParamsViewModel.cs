using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Services.Interfaces;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigArithParams.ViewModels
{
    public class ConfigArithParamsViewModel : RegionViewModelBase
    {
        private ICommunication comm;
        private PostProcParams postProcParams;
        private CorrectionParams corrParams;
        private bool isExpert;
        public bool IsExpert
        {
            get { return isExpert; }
            set { isExpert = value; RaisePropertyChanged(); }
        }

        public ConfigArithParamsViewModel(IInitArithParams initArithParams, ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            corrParams = initArithParams.InitCorrection();
            postProcParams = initArithParams.InitPostProc();
            this.comm = communication;
            this.EventAggregator.GetEvent<ConfigArithParamsRequestEvent>().Subscribe(ConfigArithParams, ThreadOption.PublisherThread, true);
            this.EventAggregator.GetEvent<ConfigCameraAEChangedEvent>().Subscribe(enable => AE = enable, ThreadOption.BackgroundThread, true);
            this.EventAggregator.GetEvent<UserAccessChangedEvent>().Subscribe(type =>
            {
                if (type == UserAccessType.Expert)
                    IsExpert = true;
                else
                    IsExpert = false;
            }, true);
        }

        private async void ConfigArithParamsAsync()
        {
            await Task.Run(() =>
            {
                comm.ConfigArithParams(corrParams, postProcParams);
            });
        }

        private void ConfigArithParams()
        {
            comm.ConfigArithParams(corrParams, postProcParams);
        }

        #region OutPut
        public bool OutPointCloud
        {
            get { return corrParams.OutPutParams.OutPointCloud; }
            set
            {
                corrParams.OutPutParams.OutPointCloud = value;
                RaisePropertyChanged();
                ConfigArithParamsAsync();
            }
        }
        public bool OutConfidence
        {
            get { return corrParams.OutPutParams.OutConfidence; }
            set { corrParams.OutPutParams.OutConfidence = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public PointCloudTypeE OutPointCloudType
        {
            get { return corrParams.OutPutParams.OutPointCloudType; }
            set { corrParams.OutPutParams.OutPointCloudType = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public DepthValueTypeE OutDepthValueType
        {
            get { return corrParams.OutPutParams.OutDepthValueType; }
            set { corrParams.OutPutParams.OutDepthValueType = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public DepthDataTypeE OutDepthDataType
        {
            get { return corrParams.OutPutParams.OutDepthDataType; }
            set { corrParams.OutPutParams.OutDepthDataType = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        #endregion

        #region Correction
        public bool CorrBP
        {
            get { return corrParams.CorrParams.CorrBP; }
            set { corrParams.CorrParams.CorrBP = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool CorrLens
        {
            get { return corrParams.CorrParams.CorrLens; }
            set { corrParams.CorrParams.CorrLens = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool CorrTemp
        {
            get { return corrParams.CorrParams.CorrTemp; }
            set { corrParams.CorrParams.CorrTemp = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool CorrOffsetAuto
        {
            get { return corrParams.CorrParams.CorrOffsetAuto; }
            set { corrParams.CorrParams.CorrOffsetAuto = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool CorrFPPN
        {
            get { return corrParams.CorrParams.CorrFPPN; }
            set { corrParams.CorrParams.CorrFPPN = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool CorrWig
        {
            get { return corrParams.CorrParams.CorrWig; }
            set { corrParams.CorrParams.CorrWig = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool CorrFPN
        {
            get { return corrParams.CorrParams.CorrFPN; }
            set { corrParams.CorrParams.CorrFPN = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool FillInvalidPixels
        {
            get { return corrParams.CorrParams.FillInvalidPixels; }
            set { corrParams.CorrParams.FillInvalidPixels = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool CutInvalidPixels
        {
            get { return corrParams.CorrParams.CutInvalidPixels; }
            set { corrParams.CorrParams.CutInvalidPixels = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool CorrOffsetManual
        {
            get { return corrParams.CorrParams.CorrOffsetManual; }
            set { corrParams.CorrParams.CorrOffsetManual = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
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
                ConfigArithParamsAsync();
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
                ConfigArithParamsAsync();
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
                ConfigArithParamsAsync();
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
                ConfigArithParamsAsync();
            }
        }
        #endregion

        #region Fusion
        public bool SFDeAliasing
        {
            get { return corrParams.FusionParams.SFDeAliasing; }
            set { corrParams.FusionParams.SFDeAliasing = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
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
                ConfigArithParamsAsync();
            }
        }

        public bool DetectWB
        {
            get { return corrParams.FusionParams.DetectWB; }
            set { corrParams.FusionParams.DetectWB = value;RaisePropertyChanged();ConfigArithParamsAsync(); }
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
                    ConfigArithParamsAsync();
                }
            }
        }
        public bool AntiAliCorr
        {
            get { return corrParams.OthersParams.AntiAliCorr; }
            set { corrParams.OthersParams.AntiAliCorr = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        #endregion

        #region PostProcOutPut
        public bool OutPointCloudPP
        {
            get { return postProcParams.OutPutParams.OutPointCloud; }
            set { postProcParams.OutPutParams.OutPointCloud = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool OutConfidencePP
        {
            get { return postProcParams.OutPutParams.OutConfidence; }
            set { postProcParams.OutPutParams.OutConfidence = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool OutFlag
        {
            get { return postProcParams.OutPutParams.OutFlag; }
            set { postProcParams.OutPutParams.OutFlag = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public PointCloudTypeE OutPointCloudTypePP
        {
            get { return postProcParams.OutPutParams.OutPointCloudType; }
            set { postProcParams.OutPutParams.OutPointCloudType = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public DepthValueTypeE OutDepthValueTypePP
        {
            get { return postProcParams.OutPutParams.OutDepthValueType; }
            set { postProcParams.OutPutParams.OutDepthValueType = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public DepthDataTypeE OutDepthDataTypePP
        {
            get { return postProcParams.OutPutParams.OutDepthDataType; }
            set { postProcParams.OutPutParams.OutDepthDataType = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        #endregion

        #region Denoising
        public DenoiseLevelE TDenoiseLevel
        {
            get { return postProcParams.DenoisingParams.TDenoiseLevel; }
            set { postProcParams.DenoisingParams.TDenoiseLevel = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public DenoiseLevelE SDenoiseLevel
        {
            get { return postProcParams.DenoisingParams.SDenoiseLevel; }
            set { postProcParams.DenoisingParams.SDenoiseLevel = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public SDenoiseMethodE SDenoiseMethod
        {
            get { return postProcParams.DenoisingParams.SDenoiseMethod; }
            set { postProcParams.DenoisingParams.SDenoiseMethod = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        #endregion

        #region Repair
        public bool DeFlyPixel
        {
            get { return postProcParams.RepairParams.DeFlyPixel; }
            set { postProcParams.RepairParams.DeFlyPixel = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool DeHoles
        {
            get { return postProcParams.RepairParams.DeHoles; }
            set { postProcParams.RepairParams.DeHoles = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        #endregion

        #region AntiInterference
        public bool AntiALI
        {
            get { return postProcParams.AntiInterferenceParams.AntiALI; }
            set { postProcParams.AntiInterferenceParams.AntiALI = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
        }
        public bool AntiMCI
        {
            get { return postProcParams.AntiInterferenceParams.AntiMCI; }
            set { postProcParams.AntiInterferenceParams.AntiMCI = value; RaisePropertyChanged(); ConfigArithParamsAsync(); }
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
                ConfigArithParamsAsync();
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
                ConfigArithParamsAsync();
            }
        }
        #endregion
    }
}
