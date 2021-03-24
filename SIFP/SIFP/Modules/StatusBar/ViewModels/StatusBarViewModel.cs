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
using System.Threading.Tasks;

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

        private string camChipID;
        public string CamChipID
        {
            get { return camChipID; }
            set { camChipID = value; RaisePropertyChanged(); }
        }

        private string camName ;
        public string CamName
        {
            get { return camName; }
            set { camName = value; RaisePropertyChanged(); }
        }

        private float sensorTemp ;
        public float SensorTemp
        {
            get { return sensorTemp; }
            set { sensorTemp = value; RaisePropertyChanged(); }
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
            set { workMode = value;RaisePropertyChanged(); }
        }

        public StatusBarViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            this.EventAggregator.GetEvent<NoticeLogEvent>().Subscribe(log => this.Log = log);

            this.EventAggregator.GetEvent<ConnectCameraReplyEvent>().Subscribe(reply =>
            {
                CamChipID = "0x" + reply.CamChipID.ToString("x2");
                CamName = reply.CamName.Split('\0')[0];
            });

            this.EventAggregator.GetEvent<ConfigCameraReplyEvent>().Subscribe(reply =>
            {
                Resolution = reply.OutImageWidth + "*" + (reply.OutImageHeight - reply.AddInfoLines);
            });

            this.EventAggregator.GetEvent<ConfigWorkModeSuceessEvent>().Subscribe(reply =>
            {
                WorkMode = reply;
            });
        }
    }
}
