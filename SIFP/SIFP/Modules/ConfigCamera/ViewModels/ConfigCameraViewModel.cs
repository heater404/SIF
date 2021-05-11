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
        public Tuple<UInt32, UInt32> IntegrationTimeRange { get; set; }
        //WorkMode的集合用于后台的绑定
        public List<ComboBoxViewMode<WorkModeE>> WorkModes { get; set; } = new List<ComboBoxViewMode<WorkModeE>>();
        //SubWorkMode的集合用于后台的绑定 这个集合是根据WorkMode动态生成的
        public List<ComboBoxViewMode<SubWorkModeE>> SubWorkModes { get; set; } = new List<ComboBoxViewMode<SubWorkModeE>>();

        public ConfigCameraViewModel(IInitCamera initCamera, IDialogService dialogService, ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager, eventAggregator)
        {
            this.comm = communication;
            this.dialogService = dialogService;
            IntegrationTimeRange = initCamera.InitIntegrationTimesRange();
            Frequencies = initCamera.InitFrequencies();
            configCameraModel = initCamera.InitConfigCamera(SubWorkModeE._4PHASE_GRAY_4PHASE_BG);
            InitWorkModes();
            Resolution = CalculateResolution(ROISize, XStep, YStep);

            ApplyConfigCameraCmd = new DelegateCommand(ApplyConfigCameraAsync);

            this.EventAggregator.GetEvent<ConfigCameraRequestEvent>().Subscribe(ApplyConfigCamera, ThreadOption.PublisherThread, true);

            this.EventAggregator.GetEvent<IsStreamingEvent>().Subscribe(isStreaming => IsEnable = !isStreaming, ThreadOption.BackgroundThread, true);

            this.EventAggregator.GetEvent<ConfigCorrectionAEChangedEvent>().Subscribe(enable => EnableAE = enable, ThreadOption.BackgroundThread, true);

            this.EventAggregator.GetEvent<ConfigArithParamsReplyEvent>().Subscribe(reply =>
            {
                if (reply.AEAck == 0)
                    this.IntegrationTimes = reply.IntegrationTimes;
                else
                    this.PrintWatchLog("AE Fail", LogLevel.Error);

            }, ThreadOption.BackgroundThread, true);

            this.EventAggregator.GetEvent<ConnectCameraReplyEvent>().Subscribe(reply =>
            {
                MaxImageSize = new Size(reply.MaxImageWidth, reply.MaxImageHeight);
            }, true);
        }

        private bool isEnable = true;
        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; RaisePropertyChanged(); }
        }

        private bool enableAE;
        public bool EnableAE
        {
            get { return enableAE; }
            set
            {
                if (value != enableAE)
                {
                    this.EventAggregator.GetEvent<ConfigCameraAEChangedEvent>().Publish(value);
                    enableAE = value;
                    RaisePropertyChanged();
                }
            }
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
            ConfigCameraSuccess = false;
            dialogService.Show(DialogNames.WaitingDialog);
            var res = await Task.Run(() => comm.ConfigCamera(configCameraModel , 5000));
            if (res.HasValue)
            {
                if (res.Value)
                {
                    this.PrintNoticeLog("ConfigCamera Success", LogLevel.Warning);
                    this.PrintWatchLog("ConfigCamera Success", LogLevel.Warning);
                    this.EventAggregator.GetEvent<ConfigWorkModeSuceessEvent>().Publish(this.SubWorkMode);
                    ConfigCameraSuccess = true;
                }
                else
                {
                    this.PrintNoticeLog("ConfigCamera Fail", LogLevel.Error);
                    this.PrintWatchLog("ConfigCamera Fail", LogLevel.Error);
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
                if (res.Value)
                {
                    this.PrintNoticeLog("ConfigCamera Success", LogLevel.Warning);
                    this.PrintWatchLog("ConfigCamera Success", LogLevel.Warning);
                    this.EventAggregator.GetEvent<ConfigWorkModeSuceessEvent>().Publish(this.SubWorkMode);
                    ConfigCameraSuccess = true;
                }
                else
                {
                    this.PrintNoticeLog("ConfigCamera Fail", LogLevel.Error);
                    this.PrintWatchLog("ConfigCamera Fail", LogLevel.Error);
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

        /// <summary>
        /// 初始化工作模式和SubWorkMode
        /// </summary>
        private void InitWorkModes()
        {
            if (configCameraModel == null)
                return;

            foreach (var usercase in configCameraModel.UserCases)
            {
                var wm = new ComboBoxViewMode<WorkModeE> { Description = usercase.WorkMode.ToString(), SelectedModel = usercase.WorkMode, IsShow = Visibility.Visible };
                if (!WorkModes.Exists(item => item.SelectedModel == wm.SelectedModel))
                    WorkModes.Add(wm);

                var swm = new ComboBoxViewMode<SubWorkModeE> { Description = usercase.SubWorkMode.ToString(), SelectedModel = usercase.SubWorkMode, IsShow = Visibility.Visible };
                if (!SubWorkModes.Exists(item => item.SelectedModel == swm.SelectedModel))
                    SubWorkModes.Add(swm);
            }

            //初始化的时候默认的SubWorkMode,其实是根据默认的WorkMode
            FilterSubWorkMode((WorkModeE)WorkMode);
        }

        public DelegateCommand ApplyConfigCameraCmd { get; set; }

        public WorkModeE WorkMode
        {
            get { return configCameraModel.CurrentUserCase.WorkMode; }
            set
            {
                FilterSubWorkMode(value);
                RaisePropertyChanged();
            }
        }
        //被选中的SubWorkMode的索引
        public SubWorkModeE SubWorkMode
        {
            get { return configCameraModel.CurrentUserCase.SubWorkMode; }
            set
            {
                configCameraModel.CurrentUserCase = configCameraModel.UserCases.Find(usercase => usercase.SubWorkMode == value);

                SubFrameModes = configCameraModel.CurrentUserCase.SubFrameModes;
                IntegrationTimes = configCameraModel.CurrentUserCase.IntegrationTimes;
                PLLDLLDivs = configCameraModel.CurrentUserCase.PLLDLLDivs;
                NumSubFramePerFrame = configCameraModel.CurrentUserCase.NumSubFramePerFrame;
                SubFrameModes = configCameraModel.CurrentUserCase.SubFrameModes;
                SpecialFrameModes = configCameraModel.CurrentUserCase.SpecialFrameModes;
                MaxFPS = configCameraModel.CurrentUserCase.MaxFPS;
                RaisePropertyChanged();
            }
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
        public UInt32 MaxFPS
        {
            get { return configCameraModel.CurrentUserCase.MaxFPS; }
            set { configCameraModel.CurrentUserCase.MaxFPS = value; RaisePropertyChanged(); }
        }
        //深度帧帧率

        public UInt32 FPS
        {
            get { return configCameraModel.DepthFPS; }
            set
            {
                configCameraModel.DepthFPS = value;
                RaisePropertyChanged();
            }
        }
        public Point StartPoint
        {
            get
            {
                return new Point(configCameraModel.ROISetting.XStart,
                    configCameraModel.ROISetting.XStart);
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
                if (value.Width < 0 || value.Width < 0)
                    throw new ArgumentException($"invalid value");
                if (StartPoint.X + value.Width > maxImageSize.Width)
                    throw new ArgumentException($"{StartPoint.X} + {value.Width} > {maxImageSize.Width}");
                if (StartPoint.Y + value.Height > maxImageSize.Height)
                    throw new ArgumentException($"{StartPoint.Y} + {value.Height} > {maxImageSize.Height}");

                configCameraModel.ROISetting.XSize = (UInt16)value.Width;
                configCameraModel.ROISetting.YSize = (UInt16)value.Height;
                RaisePropertyChanged();
                Resolution = CalculateResolution(ROISize, XStep, YStep);
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
                Resolution = CalculateResolution(ROISize, XStep, YStep);
            }
        }
        public UInt16 YStep
        {
            get { return configCameraModel.ROISetting.YStep; }
            set
            {
                if (value < 1 || value > 32)
                    throw new ArgumentOutOfRangeException("OutOfRange:[1,32]");
                configCameraModel.ROISetting.YStep = value;
                RaisePropertyChanged();
                Resolution = CalculateResolution(ROISize, XStep, YStep);
            }
        }
        public bool AnalogBinning
        {
            get
            {
                if (configCameraModel.BinningMode == BinningModeE.Analog ||
                    configCameraModel.BinningMode == BinningModeE.Both)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    if (DigitalBinning)
                        configCameraModel.BinningMode = BinningModeE.Both;
                    else
                        configCameraModel.BinningMode = BinningModeE.Analog;
                }
                else
                {
                    if (DigitalBinning)
                        configCameraModel.BinningMode = BinningModeE.Digital;
                    else
                        configCameraModel.BinningMode = BinningModeE.None;
                }
                RaisePropertyChanged();
                Resolution = CalculateResolution(ROISize, XStep, YStep);
            }
        }
        public bool DigitalBinning
        {
            get
            {
                if (configCameraModel.BinningMode == BinningModeE.Digital ||
                    configCameraModel.BinningMode == BinningModeE.Both)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    if (AnalogBinning)
                        configCameraModel.BinningMode = BinningModeE.Both;
                    else
                        configCameraModel.BinningMode = BinningModeE.Digital;
                }
                else
                {
                    if (AnalogBinning)
                        configCameraModel.BinningMode = BinningModeE.Analog;
                    else
                        configCameraModel.BinningMode = BinningModeE.None;
                }
                RaisePropertyChanged();
                Resolution = CalculateResolution(ROISize, XStep, YStep);
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

        /// <summary>
        /// 根据WorkMode动态过滤SubWorkMode，在SubWorkMode中，如果属于被选中的WorkMode则把Visibility设置为True，否则为false
        /// </summary>
        /// <param name="workMode">被选中的WorkMode</param>
        private void FilterSubWorkMode(WorkModeE workMode)
        {
            foreach (var item in SubWorkModes)
            {
                SubWorkModeE subWorkMode = item.SelectedModel;

                if (subWorkMode.GetTAttribute<SubWorkModeAttribute>().WorkModeType != workMode)
                    item.IsShow = Visibility.Collapsed;
                else
                    item.IsShow = Visibility.Visible;
            }

            //切换SubWorkMode后需要重新选择,而且默认选择第一个。
            SubWorkMode = SubWorkModes.First(swm => swm.IsShow == Visibility.Visible).SelectedModel;
        }

        private Size resolution;
        public Size Resolution
        {
            get { return resolution; }
            set { resolution = value; RaisePropertyChanged(); }
        }
        private Size CalculateResolution(Size roi, UInt16 xstep, UInt16 ystep)
        {
            uint width = (UInt16)((roi.Width + xstep - 1) / xstep / 4) * 4u;

            uint height = (UInt16)((roi.Height + ystep - 1) / ystep);

            return new Size(width, height);
        }
    }
}
