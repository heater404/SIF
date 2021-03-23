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
        public ConfigCameraViewModel(IDialogService dialogService, ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            this.comm = communication;
            this.dialogService = dialogService;
            InitDefaultConfigCamera();
            InitConfigs();
            InitWorkMode();

            FourBgSyncCmd = new DelegateCommand(FourBgSync);
            ApplyConfigCameraCmd = new DelegateCommand(ApplyConfigCamera);

            this.EventAggregator.GetEvent<ConfigCameraRequestEvent>().Subscribe(ApplyConfigCamera);
        }

        private async void ApplyConfigCamera()
        {
            dialogService.Show(DialogNames.WaitingDialog);
            var res = await Task.Run(() => comm.ConfigCamera(GetCurrentConfig(), 5000));
            if (res.HasValue)
            {
                if (res.Value)
                {
                    this.PrintNoticeLog("ConfigCamera Success", LogLevel.Warning);
                    this.PrintWatchLog("ConfigCamera Success", LogLevel.Warning);
                    this.EventAggregator.GetEvent<ConfigWorkModeSuceessEvent>().Publish(this.subWorkModeIndex);
                }
                else
                {
                    this.PrintNoticeLog("ConfigCamera Fail", LogLevel.Error);
                    this.PrintWatchLog("ConfigCamera Fail", LogLevel.Error);
                }
            }
            else
            {
                this.PrintNoticeLog("ConfigCamera Timeout", LogLevel.Error);
                this.PrintWatchLog("ConfigCamera Timeout", LogLevel.Error);
            }
            EventAggregator.GetEvent<CloseWaitingDialogEvent>().Publish();
        }

        private ConfigCameraRequest GetCurrentConfig()
        {
            SubWorkModeAttribute attribute = ((SubWorkModeE)subWorkModeIndex).GetTAttribute<SubWorkModeAttribute>();

            ConfigCameraRequest request = new ConfigCameraRequest
            {
                ConfigCamera = new ConfigCameraModel
                {
                    DoReset = config.DoReset,
                    StandByMode = config.StandByMode,
                    SysXtalClkKHz = config.SysXtalClkKHz,
                    WorkMode = this.WorkModeIndex,
                    SubWorkMode = this.SubWorkModeIndex,
                    SubFrameModes = config.SubFrameModes,
                    SpecialFrameModes = config.SpecialFrameModes,
                    DifferentialBG = config.DifferentialBG,
                    FrameSeqSchedule = config.FrameSeqSchedule,
                    IntegrationTimes = this.integrationTimes,
                    PLLDLLDivs = this.pLLDLLDivs,
                    NumSubFramePerFrame = config.NumSubFramePerFrame,
                    NumDepthSequencePerDepthMap = config.NumDepthSequencePerDepthMap,
                    MIPI_FS_FE_Pos = config.MIPI_FS_FE_Pos,
                    MIPIFrameRate = this.fps * attribute.NumDepthMapPerDepth,
                    SequencerRepeatMode = config.SequencerRepeatMode,
                    TriggerMode = config.TriggerMode,
                    ROISetting = this.GetCurrentROISetting(),
                    BinningMode = config.BinningMode,
                    MirrorMode = this.MirrorMode,
                    TSensorMode = config.TSensorMode,
                    PerformClkChanges = config.PerformClkChanges,
                    ClkDivOverride = config.ClkDivOverride,
                }
            };
            return request;
        }

        private ROISetting GetCurrentROISetting()
        {
            string roi=string.Empty;
            Application.Current.Dispatcher.Invoke(() =>
            {
                roi = selectedResolution.Content.ToString();
            });

            var roiSetting = new ROISetting
            {
                XStart = config.ROISetting.XStart,
                XSize = UInt16.Parse(roi.Split('*')[0]),
                XStep = config.ROISetting.XStep,
                YStart = config.ROISetting.YStart,
                YSize = UInt16.Parse(roi.Split('*')[1]),
                YStep = config.ROISetting.YStep,
            };

            return roiSetting;
        }

        private void FourBgSync()
        {
            if (subWorkModeIndex.GetTAttribute<SubWorkModeAttribute>().IsAsync)
            {
                IntegrationTimes[1].SpecialPhaseInt = IntegrationTimes[0].Phase1_4Int;
                IntegrationTimes[3].SpecialPhaseInt = IntegrationTimes[2].Phase1_4Int;
            }
        }

        /// <summary>
        /// 初始化工作模式和SubWorkMode
        /// </summary>
        private void InitWorkMode()
        {
            foreach (WorkModeE item in Enum.GetValues(typeof(WorkModeE)))
            {
                WorkModes.Add(new ComboBoxItemMode<WorkModeE> { Description = item.ToString(), SelectedModel = item, IsShow = Visibility.Visible });
            };

            foreach (SubWorkModeE item in Enum.GetValues(typeof(SubWorkModeE)))
            {
                if (item == SubWorkModeE._4PHASE || item == SubWorkModeE._4PHASE_BG
                    || item == SubWorkModeE._4PHASE_GRAY || item == SubWorkModeE._8PHASE
                    || item == SubWorkModeE._4PHASE_GRAY_4PHASE_BG || item == SubWorkModeE._4PHASE_4PHASE_4PHASE
                    || item == SubWorkModeE._4PHASE_4PHASE_4PHASE_4PHASE)
                    SubWorkModes.Add(new ComboBoxItemMode<SubWorkModeE> { Description = item.ToString(), SelectedModel = item, IsShow = Visibility.Visible });
            };

            //初始化的时候默认的SubWorkMode,其实是根据默认的WorkMode
            FilterSubWorkMode((WorkModeE)workModeIndex);
        }

        public DelegateCommand ApplyConfigCameraCmd { get; set; }
        public DelegateCommand FourBgSyncCmd { get; set; }

        private void InitConfigs()
        {
            GetFrequencies();
            GetIntegrationTimeRange();
        }

        private void InitDefaultConfigCamera()
        {
            string path = @"Configs\ConfigCamera.json";
            if (!File.Exists(path))
            {
                return;
            }

            string config = File.ReadAllText(path);

            defaultConfig = JsonSerializer.Deserialize<List<ConfigCameraModel>>(config);
        }

        private List<ConfigCameraModel> defaultConfig = new List<ConfigCameraModel>();
        private ConfigCameraModel config = null;

        //WorkMode的集合用于后台的绑定
        public List<ComboBoxItemMode<WorkModeE>> WorkModes { get; set; } = new List<ComboBoxItemMode<WorkModeE>>();

        //SubWorkMode的集合用于后台的绑定 这个集合是根据WorkMode动态生成的
        public List<ComboBoxItemMode<SubWorkModeE>> SubWorkModes { get; set; } = new List<ComboBoxItemMode<SubWorkModeE>>();

        //被选中的WorkMode的索引
        private WorkModeE workModeIndex = WorkModeE.SINGLE_FREQ;//决定初始化的时候是的WorkMode

        public WorkModeE WorkModeIndex
        {
            get { return workModeIndex; }

            set
            {
                workModeIndex = value;
                RaisePropertyChanged();

                //每一次切换WorkMode之后都需要更新SubWorkMode
                FilterSubWorkMode((WorkModeE)value);
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
                SubWorkModeE subWorkMode = (SubWorkModeE)Enum.Parse(typeof(SubWorkModeE), item.Description);

                if (subWorkMode.GetTAttribute<SubWorkModeAttribute>().WorkModeType != workMode)
                    item.IsShow = Visibility.Collapsed;
                else
                    item.IsShow = Visibility.Visible;
            }

            //切换SubWorkMode后需要重新选择,而且默认选择第一个。
            SubWorkModeIndex = SubWorkModes.First(s => s.IsShow == Visibility.Visible).SelectedModel;
        }

        //被选中的SubWorkMode的索引
        private SubWorkModeE subWorkModeIndex;

        public SubWorkModeE SubWorkModeIndex
        {
            get { return subWorkModeIndex; }

            set
            {
                subWorkModeIndex = value;
                RaisePropertyChanged();

                //每一次切换SubWorkMode之后都需要更新Config
                SetConfigs((SubWorkModeE)value);
            }
        }

        /// <summary>
        /// 通过SubWorkMode配置Configs的数量
        /// </summary>
        /// <param name="subWorkMode">被选中的SubWorkMode</param>
        private void SetConfigs(SubWorkModeE subWorkMode)
        {
            this.config = defaultConfig.Find(c => c.SubWorkMode == subWorkMode);

            if (config == null)
                return;

            //得到默认配置后,需要给界面绑定的值赋值,那么给DS发送ConfigCamera指令的时候,被绑定的值使用绑定的值，没有绑定的值还是使用config中的值
            SetBindingData();
            SubWorkModeAttribute attribute = ((SubWorkModeE)subWorkModeIndex).GetTAttribute<SubWorkModeAttribute>();

            this.MaxFPS = Math.Min(30, 240 / attribute.NumPhasePerDepthMap / attribute.NumDepthMapPerDepth);
            //this.FPS = this.maxFPS;
        }

        private void SetBindingData()
        {
            this.IntegrationTimes = config.IntegrationTimes;
            this.PLLDLLDivs = config.PLLDLLDivs;
            this.FPS = config.MIPIFrameRate;
            //this.BinningMode = config.BinningMode;
            //this.MirrorMode = config.MirrorMode;
            this.NumSubFramePerFrame = config.NumSubFramePerFrame;
            this.SubFrameModes = config.SubFrameModes;
            this.SpecialFrameModes = config.SpecialFrameModes;
        }

        private IntegrationTime[] integrationTimes;
        public IntegrationTime[] IntegrationTimes
        {
            get { return integrationTimes; }
            set { integrationTimes = value; RaisePropertyChanged(); }
        }

        private PLLDLLDiv[] pLLDLLDivs;//这里的单位都是KHz
        public PLLDLLDiv[] PLLDLLDivs
        {
            get { return pLLDLLDivs; }
            set { pLLDLLDivs = value; RaisePropertyChanged(); }
        }

        private UInt32[] numSubFramePerFrame;
        public UInt32[] NumSubFramePerFrame
        {
            get { return numSubFramePerFrame; }
            set { numSubFramePerFrame = value; RaisePropertyChanged(); }
        }

        private SubFrameModeE[] subFrameModes;
        public SubFrameModeE[] SubFrameModes
        {
            get { return subFrameModes; }
            set { subFrameModes = value; RaisePropertyChanged(); }
        }

        private SpecialFrameModeE[] specialFrameModes;
        public SpecialFrameModeE[] SpecialFrameModes
        {
            get { return specialFrameModes; }
            set { specialFrameModes = value; RaisePropertyChanged(); }
        }

        //用于绑定到ComboBox的以供选择的频率值,单位MHz。从配置文件读取
        public List<ComboBoxItemMode<UInt32>> Frequencies { get; set; } = new List<ComboBoxItemMode<UInt32>>();

        //积分时间的上下限，用于绑定到slider上。从配置文件获取
        public Tuple<UInt32, UInt32> IntegrationTimeRange { get; set; }

        /*DLL(output of DLL is fMod) frequency div
        clock path:
        |PLL| -> |PLL_DLL div| -> |div by 2 fixed| -> DLL(fMod)

        e.g. for PHASE1_4's modulation frequency:
        The final division ratio from PLL to output of DLL(fMod) is PHASE1_4_PLL_DLL_DIV * 2
        So, fMod = fPLL / (PHASE1_4_PLL_DLL_DIV * 2)

        IMPORTANT: allowed values for the 3 values in the struct are:
        2,3,4,5 and from 6 to 30(only even numbers)
        2的时候频率太高不需要
        */
        private void GetFrequencies()
        {
            string fre = "990";// AppConfigHelper.GetAppConfigValue("PllFrequency");

            double pllFreq = double.Parse(fre);//Pll频率 

            for (uint i = 3; i < 6; i++)//分频 2的时候频率太高不需要
            {
                Frequencies.Add(new ComboBoxItemMode<UInt32> { Description = (pllFreq / i / 2.0).ToString("0.00"), SelectedModel = i, IsShow = Visibility.Visible });
            }
            for (uint i = 6; i <= 30; i++)//分频
            {
                if (i % 2 == 0)
                    Frequencies.Add(new ComboBoxItemMode<UInt32> { Description = (pllFreq / i / 2.0).ToString("0.00"), SelectedModel = i, IsShow = Visibility.Visible });
            }
        }

        private void GetIntegrationTimeRange()
        {
            string range = "1,1500";// AppConfigHelper.GetAppConfigValue("IntegrationTimeRange");

            string[] ranges = range.Split(',');

            IntegrationTimeRange = new Tuple<uint, uint>(uint.Parse(ranges[0]), uint.Parse(ranges[1]));
        }

        private ComboBoxItem selectedResolution;
        public ComboBoxItem SelectedResolution
        {
            get { return selectedResolution; }
            set { selectedResolution = value; RaisePropertyChanged(); }
        }

        private UInt32 fps = 25;

        public UInt32 FPS
        {
            get { return fps; }
            set { fps = value; RaisePropertyChanged(); }
        }

        private UInt32 maxFPS;
        public UInt32 MaxFPS
        {
            get { return maxFPS; }
            set { maxFPS = value; RaisePropertyChanged(); }
        }

        private bool horizontalMirror;

        public bool HorizontalMirror
        {
            get { return horizontalMirror; }
            set { horizontalMirror = value; RaisePropertyChanged(); }
        }

        private bool verticalMirror;

        public bool VerticalMirror
        {
            get { return verticalMirror; }
            set { verticalMirror = value; RaisePropertyChanged(); }
        }

        public MirrorModeE MirrorMode
        {
            get
            {
                if (horizontalMirror && verticalMirror)
                    return MirrorModeE.Both;
                else if (horizontalMirror)
                    return MirrorModeE.Horizontal;
                else if (verticalMirror)
                    return MirrorModeE.Vertical;
                else
                    return MirrorModeE.None;
            }
            set
            {
                if (value == MirrorModeE.None)
                {
                    horizontalMirror = false;
                    verticalMirror = false;
                }
                else if (value == MirrorModeE.Horizontal)
                {
                    horizontalMirror = true;
                    verticalMirror = false;
                }
                else if (value == MirrorModeE.Vertical)
                {
                    verticalMirror = true;
                    horizontalMirror = false;
                }
                else
                {
                    horizontalMirror = true;
                    verticalMirror = true;
                }
            }
        }
    }
}
