using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Services.Interfaces;
using SIFP.Core;
using SIFP.Core.Attributes;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ConfigCamera.ViewModels
{
    public class ConfigCameraViewModel : RegionViewModelBase
    {
        private ICommunication comm;
        private IDialogService dialogService;
        private ConfigCameraModel configCameraModel;

        private Size maxImageSize = new Size(640, 480);
        public Size MaxImageSize
        {
            get { return maxImageSize; }
            set { maxImageSize = value; RaisePropertyChanged(); }
        }
        //用于绑定到ComboBox的以供选择的频率值,单位MHz。从配置文件读取
        public List<ComboBoxViewMode<UInt32>> Frequencies { get; set; } = new List<ComboBoxViewMode<UInt32>>();
        //积分时间的上下限，用于绑定到slider上。从配置文件获取
        public Tuple<double, double> IntegrationTimeRange { get; set; }
        //WorkMode的集合用于后台的绑定
        public List<ComboBoxViewMode<WorkModeE>> WorkModes { get; set; } = new List<ComboBoxViewMode<WorkModeE>>();
        //SubWorkMode的集合用于后台的绑定 这个集合是根据WorkMode动态生成的
        public List<ComboBoxViewMode<SubWorkModeE>> SubWorkModes { get; set; } = new List<ComboBoxViewMode<SubWorkModeE>>();

        public List<ComboBoxViewMode<BinningModeE>> BinningModes { get; set; } = new List<ComboBoxViewMode<BinningModeE>>();

        public List<ComboBoxViewMode<SubFrameModeE>> SubFrameModesE { get; set; } = new List<ComboBoxViewMode<SubFrameModeE>>();
        public List<ComboBoxViewMode<SpecialFrameModeE>> SpecialFrameModesE { get; set; } = new List<ComboBoxViewMode<SpecialFrameModeE>>();

        public ConfigCameraViewModel(IInitCamera initCamera, IDialogService dialogService, ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager, eventAggregator)
        {
            this.comm = communication;
            this.dialogService = dialogService;
            //IntegrationTimeRange = initCamera.InitIntegrationTimesRange();
            Frequencies = initCamera.InitFrequencies();
            //configCameraModel = initCamera.InitConfigCamera(SubWorkModeE._4PHASE_GRAY_4PHASE_BG);
            //InitWorkModes();
            InitConfigCameraModel();

            InitBinningModes();

            foreach (SubFrameModeE item in Enum.GetValues(typeof(SubFrameModeE)))
            {
                var mode = new ComboBoxViewMode<SubFrameModeE>
                {
                    Description = item.ToString(),
                    SelectedModel = item,
                    IsShow = Visibility.Visible,
                };
                SubFrameModesE.Add(mode);
            }

            foreach (SpecialFrameModeE item in Enum.GetValues(typeof(SpecialFrameModeE)))
            {
                var mode = new ComboBoxViewMode<SpecialFrameModeE>
                {
                    Description = item.ToString(),
                    SelectedModel = item,
                    IsShow = Visibility.Visible,
                };
                SpecialFrameModesE.Add(mode);
            }

            Resolution = CalculateResolution(ROISize, XStep, YStep, configCameraModel.BinningMode);

            ApplyConfigCameraCmd = new DelegateCommand(ApplyConfigCameraAsync);
            BinningModeSelectedCmd = new DelegateCommand<int?>(BinningModeSelected);

            this.EventAggregator.GetEvent<ConfigCameraRequestEvent>().Subscribe(ApplyConfigCamera, ThreadOption.PublisherThread, true);

            //this.EventAggregator.GetEvent<IsStreamingEvent>().Subscribe(isStreaming => IsEnable = !isStreaming, ThreadOption.BackgroundThread, true);

            //this.EventAggregator.GetEvent<ConfigCorrectionAEChangedEvent>().Subscribe(enable => EnableAE = enable, ThreadOption.BackgroundThread, true);

            this.EventAggregator.GetEvent<ConfigArithParamsReplyEvent>().Subscribe(reply =>
            {
                if (reply.AEAck == 0)
                    this.IntegrationTimes = reply.IntegrationTimes;

            }, ThreadOption.BackgroundThread, true);

            this.EventAggregator.GetEvent<ConnectCameraReplyEvent>().Subscribe(reply =>
            {
                MaxImageSize = new Size(reply.ToFMaxImageWidth, reply.ToFMaxImageHeight);
            }, true);
        }

        private void InitConfigCameraModel()
        {
            configCameraModel = new ConfigCameraModel
            {
                DoReset = 0,
                StandByMode = StandByModeE.None,
                SysXtalClkKHz = 19800,
                CurrentUserCase = new UserCaseModel
                {
                    WorkMode = WorkModeE.CUSTOM,
                    SubWorkMode = SubWorkModeE.CUSTOM,
                    SubFrameModes = new SubFrameModeE[4]
                    { SubFrameModeE.Mode_8Phase, SubFrameModeE.Mode_8Phase, SubFrameModeE.Mode_8Phase, SubFrameModeE.Mode_8Phase },
                    SpecialFrameModes = new SpecialFrameModeE[4]
                    { SpecialFrameModeE.Normal, SpecialFrameModeE.Normal, SpecialFrameModeE.Normal, SpecialFrameModeE.Normal },
                    DifferentialBG = 0,
                    FrameSeqSchedule = new FrameSeqSchedule { Slot0FrameNum = 0, Slot1FrameNum = 1, Slot2FrameNum = 2, Slot3FrameNum = 3 },
                    IntegrationTimes = new IntegrationTime[4]
                    {
                        new IntegrationTime { Phase1_4Int = 1000, Phase5_8Int = 1000, SpecialPhaseInt = 1000 },
                        new IntegrationTime { Phase1_4Int = 1000, Phase5_8Int = 1000, SpecialPhaseInt = 1000 },
                        new IntegrationTime { Phase1_4Int = 1000, Phase5_8Int = 1000, SpecialPhaseInt = 1000 },
                        new IntegrationTime { Phase1_4Int = 1000, Phase5_8Int = 1000, SpecialPhaseInt = 1000 },
                    },
                    PLLDLLDivs = new PLLDLLDiv[4]
                    {
                        new PLLDLLDiv { Phase1_4Div = 3, Phase5_8Div = 3, SpecialPhaseDiv = 3 },
                        new PLLDLLDiv { Phase1_4Div = 3, Phase5_8Div = 3, SpecialPhaseDiv = 3 },
                        new PLLDLLDiv { Phase1_4Div = 3, Phase5_8Div = 3 ,SpecialPhaseDiv = 3 },
                        new PLLDLLDiv { Phase1_4Div = 3, Phase5_8Div = 3 ,SpecialPhaseDiv = 3 },
                    },
                    NumSubFramePerFrame = new uint[4] { 1, 1, 1, 1 },
                    EnableLedMod = 1,
                    NumDepthSequencePerDepthMap = 1,
                    MIPI_FS_FE_Pos = MIPI_FS_FE_PosE.DepthMap,
                },
                DepthFPS = 10,
                SequencerRepeatMode = SequencerRepeatModeE.Auto_Repeat,
                TriggerMode = TriggerModeE.Master_Mode,
                SlaveTriggeredOnPosLevel = false,
                ROISetting = new ROISetting
                {
                    XStart = 0,
                    XSize = 640,
                    XStep = 1,
                    YStart = 0,
                    YSize = 480,
                    YStep = 1,
                },
                BinningMode = BinningModeE.None,
                MirrorMode = MirrorModeE.None,
                TSensorMode = TSensorModeE.EveryPhase,
                PerformClkChanges = 0,
                ClkDivOverride = new ClkDIvOverride
                {
                    ClkDigSlowDiv = 0,
                    PLLFBDiv = 0,
                    PLLPreDiv = 0,
                },
                PhaseMode = PhaseModeE.PhaseMode1,
                NeedReply = 1,
            };
        }
        public SubFrameModeE[] SubFrameModes
        {
            get { return configCameraModel.CurrentUserCase.SubFrameModes; }
            set { configCameraModel.CurrentUserCase.SubFrameModes = value; RaisePropertyChanged(); }
        }
        public SpecialFrameModeE[] SpecialFrameModes
        {
            get { return configCameraModel.CurrentUserCase.SpecialFrameModes; }
            set { configCameraModel.CurrentUserCase.SpecialFrameModes = value; RaisePropertyChanged(); }
        }

        public bool DifferentialBG
        {
            get { return configCameraModel.CurrentUserCase.DifferentialBG != 0; }
            set { configCameraModel.CurrentUserCase.DifferentialBG = value ? 0 : 1; RaisePropertyChanged(); }
        }

        public FrameSeqSchedule FrameSeqSchedule
        {
            get { return configCameraModel.CurrentUserCase.FrameSeqSchedule; }
            set { configCameraModel.CurrentUserCase.FrameSeqSchedule = value; RaisePropertyChanged(); }
        }

        public IntegrationTime[] IntegrationTimes //ns
        {
            get { return configCameraModel.CurrentUserCase.IntegrationTimes; }
            set { configCameraModel.CurrentUserCase.IntegrationTimes = value; RaisePropertyChanged(); }
        }
        public PLLDLLDiv[] PLLDLLDivs//这里的单位都是KHz
        {
            get { return configCameraModel.CurrentUserCase.PLLDLLDivs; }
            set { configCameraModel.CurrentUserCase.PLLDLLDivs = value; RaisePropertyChanged(); }
        }

        public UInt32[] NumSubFramePerFrame
        {
            get { return configCameraModel.CurrentUserCase.NumSubFramePerFrame; }
            set { configCameraModel.CurrentUserCase.NumSubFramePerFrame = value; RaisePropertyChanged(); }
        }

        public bool EnableLedMod
        {
            get { return configCameraModel.CurrentUserCase.EnableLedMod == 1; }
            set { configCameraModel.CurrentUserCase.EnableLedMod = value ? 1 : 0; RaisePropertyChanged(); }
        }

        public uint NumDepthSequencePerDepthMap
        {
            get { return configCameraModel.CurrentUserCase.NumDepthSequencePerDepthMap; }
            set { configCameraModel.CurrentUserCase.NumDepthSequencePerDepthMap = value; RaisePropertyChanged(); }
        }

        public MIPI_FS_FE_PosE MIPI_FS_FE_Pos
        {
            get { return configCameraModel.CurrentUserCase.MIPI_FS_FE_Pos; }
            set { configCameraModel.CurrentUserCase.MIPI_FS_FE_Pos = value; RaisePropertyChanged(); }
        }

        public Array MIPI_FS_FE_Poss
        {
            get { return Enum.GetValues(typeof(MIPI_FS_FE_PosE)); }
        }

        public uint DepthFPS
        {
            get { return configCameraModel.DepthFPS; }
            set { configCameraModel.DepthFPS = value; RaisePropertyChanged(); }
        }
        public SequencerRepeatModeE SequencerRepeatMode
        {
            get { return configCameraModel.SequencerRepeatMode; }
            set { configCameraModel.SequencerRepeatMode = value; RaisePropertyChanged(); }
        }

        public Array SequencerRepeatModes
        {
            get { return Enum.GetValues(typeof(SequencerRepeatModeE)); }
        }

        public TriggerModeE TriggerMode
        {
            get { return configCameraModel.TriggerMode; }
            set { configCameraModel.TriggerMode = value; RaisePropertyChanged(); }
        }

        public bool SlaveTriggeredOnPosLevel
        {
            get { return configCameraModel.SlaveTriggeredOnPosLevel; }
            set { configCameraModel.SlaveTriggeredOnPosLevel = value; RaisePropertyChanged(); }
        }

        private void InitBinningModes()
        {
            foreach (BinningModeE item in Enum.GetValues(typeof(BinningModeE)))
            {
                var mode = new ComboBoxViewMode<BinningModeE>
                {
                    Description = item.ToString(),
                    SelectedModel = item,
                    IsShow = Visibility.Visible,
                };
                BinningModes.Add(mode);
            }
        }

        private void BinningModeSelected(int? selectedIndex)
        {
            if (!selectedIndex.HasValue)
                return;

            if (selectedIndex == 0)
                configCameraModel.BinningMode = BinningModeE.None;
            else if (selectedIndex == 1)
                configCameraModel.BinningMode = BinningModeE.Analog;
            else if (selectedIndex == 2)
                configCameraModel.BinningMode = BinningModeE.Digital;
            else if (selectedIndex == 3)
                configCameraModel.BinningMode = BinningModeE._2X2;
            else if (selectedIndex == 4)
                configCameraModel.BinningMode = BinningModeE._4X4;
        }


        private bool configCameraSuccess;
        public bool ConfigCameraSuccess
        {
            get { return configCameraSuccess; }
            set
            {
                configCameraSuccess = value;
                RaisePropertyChanged();
                this.EventAggregator.GetEvent<ConfigCameraSuccessEvent>().Publish(value);
            }
        }

        private async void ApplyConfigCameraAsync()
        {
            //使用analog binning时，yStart必须是偶数, yStep必须是(2 - 32)之间的偶数
            if (configCameraModel.BinningMode == BinningModeE.Analog
                    || configCameraModel.BinningMode == BinningModeE._2X2
                    || configCameraModel.BinningMode == BinningModeE._4X4)
            {
                if (StartPoint.Y % 2 != 0 || YStep % 2 != 0)
                {
                    this.PrintNoticeLog("when analogbinning,yStart and yStep must be even number", LogLevel.Error);
                    this.PrintWatchLog("when analogbinning,yStart and yStep must be even number", LogLevel.Error);
                    return;
                }
            }

            ConfigCameraSuccess = false;
            dialogService.Show(DialogNames.WaitingDialog);
            var res = await Task.Run(() => comm.ConfigCamera(configCameraModel, 5000));
            if (res.HasValue)
            {
                if (res.Value == ConfigCameraReplyE.Success)
                {
                    this.PrintNoticeLog("ConfigCamera Success", LogLevel.Warning);
                    this.PrintWatchLog("ConfigCamera Success", LogLevel.Warning);
                    //this.EventAggregator.GetEvent<ConfigWorkModeSuceessEvent>().Publish(this.SubWorkMode);
                    ConfigCameraSuccess = true;
                }
                else
                {
                    this.PrintNoticeLog($"ConfigCamera Fail:{res.Value}", LogLevel.Error);
                    this.PrintWatchLog($"ConfigCamera Fail:{res.Value}", LogLevel.Error);
                    ConfigCameraSuccess = false;
                }
            }
            else
            {
                this.PrintNoticeLog("ConfigCamera Timeout", LogLevel.Error);
                this.PrintWatchLog("ConfigCamera Timeout", LogLevel.Error);
                ConfigCameraSuccess = false;
            }
            EventAggregator.GetEvent<CloseWaitingDialogEvent>().Publish();
        }

        private void ApplyConfigCamera()
        {
            ConfigCameraSuccess = false;

            var res = comm.ConfigCamera(configCameraModel, 5000);
            if (res.HasValue)
            {
                if (res.Value == ConfigCameraReplyE.Success)
                {
                    this.PrintNoticeLog("ConfigCamera Success", LogLevel.Warning);
                    this.PrintWatchLog("ConfigCamera Success", LogLevel.Warning);
                    //this.EventAggregator.GetEvent<ConfigWorkModeSuceessEvent>().Publish(this.SubWorkMode);
                    ConfigCameraSuccess = true;
                }
                else
                {
                    this.PrintNoticeLog($"ConfigCamera Fail:{res.Value}", LogLevel.Error);
                    this.PrintWatchLog($"ConfigCamera Fail:{res.Value}", LogLevel.Error);
                    ConfigCameraSuccess = false;
                }
            }
            else
            {
                this.PrintNoticeLog("ConfigCamera Timeout", LogLevel.Error);
                this.PrintWatchLog("ConfigCamera Timeout", LogLevel.Error);
                ConfigCameraSuccess = false;
            }
        }

        public DelegateCommand ApplyConfigCameraCmd { get; set; }
        public DelegateCommand<int?> BinningModeSelectedCmd { get; set; }


        public UInt32 PhaseMode
        {
            get { return (UInt32)configCameraModel.PhaseMode; }
            set { configCameraModel.PhaseMode = (PhaseModeE)value; RaisePropertyChanged(); }
        }

        /*
         分辨率的设置有限制条件：
         1、ROI Width需要是4的倍数
         2、使用analog binning时，yStart必须是偶数, yStep必须是(2-32)之间的偶数
         3、客户版中digital Binning不能与XStep同时使用
         */
        public Point StartPoint
        {
            get
            {
                return new Point(configCameraModel.ROISetting.XStart,
                    configCameraModel.ROISetting.YStart);
            }
            set
            {
                if (value.X < 0 || value.Y < 0)
                    throw new ArgumentException($"invalid value");
                if (value.X + ROISize.Width > maxImageSize.Width)
                    throw new ArgumentException($"{value.X} + {ROISize.Width} > {maxImageSize.Width}");
                if (value.Y + ROISize.Height > maxImageSize.Height)
                    throw new ArgumentException($"{value.Y} + {ROISize.Height} > {maxImageSize.Height}");

                configCameraModel.ROISetting.XStart = (UInt16)value.X;
                configCameraModel.ROISetting.YStart = (UInt16)value.Y;
                RaisePropertyChanged();
            }
        }
        public Size ROISize
        {
            get
            {
                return new Size(configCameraModel.ROISetting.XSize,
                configCameraModel.ROISetting.YSize);
            }
            set
            {
                if (value.Width % 4 != 0) //ROI Width需要是4的倍数
                    throw new ArgumentException($"The width must be a multiple of 4");
                if (value.Width < 0 || value.Width < 0)
                    throw new ArgumentException($"invalid value");
                if (StartPoint.X + value.Width > maxImageSize.Width)
                    throw new ArgumentException($"{StartPoint.X} + {value.Width} > {maxImageSize.Width}");
                if (StartPoint.Y + value.Height > maxImageSize.Height)
                    throw new ArgumentException($"{StartPoint.Y} + {value.Height} > {maxImageSize.Height}");

                configCameraModel.ROISetting.XSize = (UInt16)value.Width;
                configCameraModel.ROISetting.YSize = (UInt16)value.Height;
                RaisePropertyChanged();
                Resolution = CalculateResolution(ROISize, XStep, YStep, this.BinningMode);
            }
        }
        public UInt16 XStep
        {
            get { return configCameraModel.ROISetting.XStep; }
            set
            {
                if (value < 1 || value > 32)
                    throw new ArgumentException("OutOfRange:[1,32]");
                configCameraModel.ROISetting.XStep = value;
                RaisePropertyChanged();
                Resolution = CalculateResolution(ROISize, XStep, YStep, this.BinningMode);
            }
        }
        public UInt16 YStep
        {
            get { return configCameraModel.ROISetting.YStep; }
            set
            {
                if (value < 1 || value > 32)
                    throw new ArgumentException("OutOfRange:[1,32]");
                configCameraModel.ROISetting.YStep = value;
                RaisePropertyChanged();
                Resolution = CalculateResolution(ROISize, XStep, YStep, this.BinningMode);
            }
        }

        public TSensorModeE TSensorMode
        {
            get { return configCameraModel.TSensorMode; }
            set { configCameraModel.TSensorMode = value; RaisePropertyChanged(); }
        }

        public BinningModeE BinningMode
        {
            get { return configCameraModel.BinningMode; }
            set
            {
                configCameraModel.BinningMode = value;
                RaisePropertyChanged();

                Resolution = CalculateResolution(ROISize, XStep, YStep, this.BinningMode);
            }
        }

        public bool HorizontalMirror
        {
            get
            {
                if (configCameraModel.MirrorMode == MirrorModeE.Horizontal ||
                    configCameraModel.MirrorMode == MirrorModeE.Both)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    if (VerticalMirror)
                        configCameraModel.MirrorMode = MirrorModeE.Both;
                    else
                        configCameraModel.MirrorMode = MirrorModeE.Horizontal;
                }
                else
                {
                    if (VerticalMirror)
                        configCameraModel.MirrorMode = MirrorModeE.Vertical;
                    else
                        configCameraModel.MirrorMode = MirrorModeE.None;
                }
                RaisePropertyChanged();
            }
        }
        public bool VerticalMirror
        {
            get
            {
                if (configCameraModel.MirrorMode == MirrorModeE.Vertical ||
                     configCameraModel.MirrorMode == MirrorModeE.Both)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    if (HorizontalMirror)
                        configCameraModel.MirrorMode = MirrorModeE.Both;
                    else
                        configCameraModel.MirrorMode = MirrorModeE.Vertical;
                }
                else
                {
                    if (HorizontalMirror)
                        configCameraModel.MirrorMode = MirrorModeE.Horizontal;
                    else
                        configCameraModel.MirrorMode = MirrorModeE.None;
                }
                RaisePropertyChanged();
            }
        }


        private Size resolution;
        public Size Resolution
        {
            get { return resolution; }
            set
            {
                resolution = value;
                RaisePropertyChanged();
                if (value.Width < 1 || value.Height < 1)
                    throw new ArgumentException($"Invalid resolution:{value}");
            }
        }
        private Size CalculateResolution(Size roi, UInt16 xstep, UInt16 ystep, BinningModeE binning)
        {
            uint xbinning = 1, ybinnig = 1;
            if (binning == BinningModeE._2X2)
            {
                xbinning = 2;
                ybinnig = 1;
            }
            else if (binning == BinningModeE.None)
            {
                xbinning = 1;
                ybinnig = 1;
            }
            else if (binning == BinningModeE.Analog)
            {
                xbinning = 1;
                ybinnig = 1;
            }
            else if (binning == BinningModeE.Digital)
            {
                xbinning = 2;
                ybinnig = 1;
            }
            else if (binning == BinningModeE._4X4)
            {
                xbinning = 4;
                ybinnig = 2;
            }


            uint width = (UInt16)((roi.Width + xstep - 1) / xstep / 4) * 4u / xbinning;

            uint height = (UInt16)((roi.Height + ystep - 1) / ystep) / ybinnig;

            return new Size(width, height);
        }
    }
}
