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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VcselDriverDialog.ViewModels
{
    public class VcselDriverViewModel : RegionViewModelBase, IDialogAware
    {
        private ICommunication comm;
        private ConfigVcselDriver vcselDriver;
        public VcselDriverViewModel(ICommunication communication, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager, eventAggregator)
        {
            this.comm = communication;

            this.vcselDriver = new ConfigVcselDriver();
            eventAggregator.GetEvent<ConfigVcselDriverReplyEvent>().Subscribe(reply =>
            {
                if (reply.Ack == 1)
                {
                    this.PrintWatchLog("ConfigVcselDriver Fail", LogLevel.Error);
                    this.PrintNoticeLog("ConfigVcselDriver Fail", LogLevel.Error);
                }
                    
                else
                {
                    this.PrintNoticeLog("ConfigVcselDriver Success", LogLevel.Warning);
                    this.PrintWatchLog("ConfigVcselDriver Success", LogLevel.Warning);
                }
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

        public Array Isws { get; } = new float[] { 0, 1.0f, 2.0f, 3.0f };

        public float Isw//A
        {
            get { return vcselDriver.Isw; }
            set { vcselDriver.Isw = value; RaisePropertyChanged(); ConfigVcselDriver(); }
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

        }
    }
}
