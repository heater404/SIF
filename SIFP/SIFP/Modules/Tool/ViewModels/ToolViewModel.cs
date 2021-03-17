using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Serilog;
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


        public ToolViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            ConnectCtrlCmd = new DelegateCommand(ConnectCtrl);

            EventAggregator.GetEvent<ConnectCameraReplyEvent>().Subscribe(RecvConnectCameraReply);
        }

        private void RecvConnectCameraReply(ConnectCameraReply reply)
        {
            
        }

        private void ConnectCtrl()
        {

        }
    }
}
