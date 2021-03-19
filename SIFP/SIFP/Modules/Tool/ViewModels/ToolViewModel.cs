using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Serilog;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool.ViewModels
{
    public class ToolViewModel : RegionViewModelBase
    {
        public DelegateCommand ConnectCtrlCmd { get; private set; }
        public DelegateCommand StreamingCtrlCmd { get; private set; }

        public DelegateCommand CaptureDataShowCmd { get; private set; }
        private IDialogService dialogService;
        public ToolViewModel(IDialogService dialogService, IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            this.dialogService = dialogService;
            CaptureDataShowCmd = new DelegateCommand(CaptureDataShow);
            ConnectCtrlCmd = new DelegateCommand(ConnectCtrl);
            StreamingCtrlCmd = new DelegateCommand(StreamingCtrl);
            EventAggregator.GetEvent<ConnectCameraReplyEvent>().Subscribe(RecvConnectCameraReply);
        }

        private void CaptureDataShow()
        {
            dialogService.ShowDialog("CaptureDataView");
        }

        private void StreamingCtrl()
        {

        }

        private void RecvConnectCameraReply(ConnectCameraReply reply)
        {

        }

        private void ConnectCtrl()
        {
            this.EventAggregator.GetEvent<WatchLogEvent>().Publish(new WatchLogModel("qwhfoiqhofiqh", WatchLogLevel.Error));
        }
    }
}
