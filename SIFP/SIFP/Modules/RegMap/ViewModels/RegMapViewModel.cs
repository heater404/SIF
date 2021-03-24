using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegMap.ViewModels
{
    public class RegMapViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public RegMapViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            Message = "View A from your Prism Module";
        }
    }
}
