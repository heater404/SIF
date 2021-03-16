using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Serilog;
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
        }

        private void ConnectCtrl()
        {

        }
    }
}
