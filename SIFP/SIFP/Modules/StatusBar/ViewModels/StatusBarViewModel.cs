using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace StatusBar.ViewModels
{
    public class StatusBarViewModel : RegionViewModelBase
    {
        private LogModel log;
        public LogModel Log
        {
            get { return log; }
            set { log = value; RaisePropertyChanged(); }
        }

        private string lotNumber;
        public string LotNumber
        {
            get { return lotNumber; }
            set { lotNumber = value; RaisePropertyChanged(); }
        }

        private string waferId;
        public string WaferId
        {
            get { return waferId; }
            set { waferId = value; RaisePropertyChanged(); }
        }

        private string camChipID;
        public string CamChipID
        {
            get { return camChipID; }
            set { camChipID = value; RaisePropertyChanged(); }
        }

        private string camName;
        public string CamName
        {
            get { return camName; }
            set { camName = value; RaisePropertyChanged(); }
        }

        private double tSensor;
        public double TSensor
        {
            get { return tSensor; }
            set { tSensor = value; RaisePropertyChanged(); }
        }

        private string resolution;
        public string Resolution
        {
            get { return resolution; }
            set { resolution = value; RaisePropertyChanged(); }
        }

        private SubWorkModeE workMode;
        public SubWorkModeE WorkMode
        {
            get { return workMode; }
            set { workMode = value; RaisePropertyChanged(); }
        }

        private bool? isConnected;
        public bool? IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                RaisePropertyChanged();

                if (value.HasValue)
                {
                    if (value.Value)//连接上了
                        beat.StartHeartBeat(new CancellationTokenSource());//开始心跳
                    else//心跳超时
                        beat.StopHeartBeat();
                }
                else//连接断开了
                    beat.StopHeartBeat();
            }
        }

        private ServerHeartBeat beat = new ServerHeartBeat(5000);
        public StatusBarViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            beat.HeartBeatTimeoutEvent += HeartBeatTimeoutEvent;

            this.EventAggregator.GetEvent<NoticeLogEvent>().Subscribe(log => this.Log = log, ThreadOption.BackgroundThread, true);

            this.EventAggregator.GetEvent<ConnectCameraReplyEvent>().Subscribe(reply =>
            {
                IsConnected = true;

                CamChipID = "0x" + reply.ToFChipID.ToString("x2");
                CamName = reply.ToFCamName.Split('\0')[0];

                LotNumber = "0x" + reply.LotNumber.ToString("x2");
                WaferId = "0x" + reply.WaferId.ToString("x2");
            }, ThreadOption.BackgroundThread, true);

            this.EventAggregator.GetEvent<ConfigCameraReplyEvent>().Subscribe(reply =>
            {
                Resolution = reply.OutImageWidth + "*" + (reply.OutImageHeight - reply.AddInfoLines);
            }, ThreadOption.BackgroundThread, true);

            this.EventAggregator.GetEvent<ConfigWorkModeSuceessEvent>().Subscribe(reply =>
            {
                WorkMode = reply;
            }, true);

            this.EventAggregator.GetEvent<GetSysStatusReplyEvent>().Subscribe(reply =>
            {
                beat.HeartBeat(DateTime.Now);
                TSensor = reply.TSensor;
            }, ThreadOption.BackgroundThread, true);

            this.EventAggregator.GetEvent<DisconnectCameraReplyEvent>().Subscribe(reply =>
            {
                IsConnected = null;
            }, ThreadOption.BackgroundThread, true);
        }

        private void HeartBeatTimeoutEvent(object sender, EventArgs e)
        {
            IsConnected = false;
            PrintNoticeLog("HeartBeatTiemout", LogLevel.Error);
            PrintWatchLog("HeartBeatTiemout", LogLevel.Error);
        }
    }
}
