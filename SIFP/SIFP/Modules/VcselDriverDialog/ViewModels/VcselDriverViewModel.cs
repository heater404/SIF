using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Services.Interfaces;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VcselDriverDialog.ViewModels
{
    public class VcselDriverViewModel : RegionViewModelBase, IDialogAware
    {
        private ICommunication comm;
        private ConfigVcselDriver vcselDriver;
        public DelegateCommand ConfigVcselDriverCmd { get; set; }
        public VcselDriverViewModel(ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager, eventAggregator)
        {
            this.comm = communication;

            this.vcselDriver = new ConfigVcselDriver();

            ConfigVcselDriverCmd = new DelegateCommand(ConfigVcselDriver);
            eventAggregator.GetEvent<ConfigVcselDriverReplyEvent>().Subscribe(reply =>
            {
                if (reply.Ack == 0)
                {
                    this.PrintWatchLog("ConfigVcselDriver Success", LogLevel.Warning);
                }
                else
                {
                    this.PrintWatchLog("ConfigVcselDriver Fail", LogLevel.Error);
                }

                MaxIBias = (UInt32)(Math.Round(reply.MaxIBiasMicroAmp / 1000.0 / TickFrequency) * TickFrequency);
                MaxISw = (UInt32)(Math.Round(reply.MaxISwitchMicroAmp / 1000.0 / TickFrequency) * TickFrequency);
                IBias = (UInt32)(Math.Round(reply.IBiasMicroAmp / 1000.0 / TickFrequency) * TickFrequency);
                ISw = (UInt32)(Math.Round(reply.ISwitchMicroAmp / 1000.0 / TickFrequency) * TickFrequency);
            }, true);
        }

        private async void ConfigVcselDriver()
        {
            await Task.Run(() =>
            {
                comm.ConfigVcselDriver(vcselDriver);
            });
        }

        public string Title => "VcselDriver";

        public event Action<IDialogResult> RequestClose;

        public double TickFrequency { get; set; } = 100.0;//100mA
        public UInt32 ISw//mA
        {
            get { return (UInt32)Math.Round(vcselDriver.ISw / 1000.0); }
            set { vcselDriver.ISw = value * 1000; RaisePropertyChanged(); }
        }
        private UInt32 maxISw;//mA
        public UInt32 MaxISw
        {
            get { return maxISw; }
            set { maxISw = value; RaisePropertyChanged(); }
        }

        public UInt32 IBias//mA
        {
            get { return (UInt32)Math.Round(vcselDriver.IBias / 1000.0); }
            set { vcselDriver.IBias = value * 1000; RaisePropertyChanged(); }
        }
        private UInt32 maxIBias;//mA
        public UInt32 MaxIBias
        {
            get { return maxIBias; }
            set { maxIBias = value; RaisePropertyChanged(); }
        }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            ConfigVcselDriverReply reply = parameters.GetValue<ConfigVcselDriverReply>("ConfigVcselDriverReply");
            MaxIBias = (UInt32)(Math.Round(reply.MaxIBiasMicroAmp / 1000.0 / TickFrequency) * TickFrequency);
            MaxISw = (UInt32)(Math.Round(reply.MaxISwitchMicroAmp / 1000.0 / TickFrequency) * TickFrequency);
            IBias = (UInt32)(Math.Round(reply.IBiasMicroAmp / 1000.0 / TickFrequency) * TickFrequency);
            ISw = (UInt32)(Math.Round(reply.ISwitchMicroAmp / 1000.0 / TickFrequency) * TickFrequency);
        }
    }
}
