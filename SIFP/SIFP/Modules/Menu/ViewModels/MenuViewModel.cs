using Menu.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Serilog;
using SIFP.Core;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.ViewModels
{
    public class MenuViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public MenuViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) :base(regionManager, eventAggregator)
        {
            Message = "MenuModule";
        }
    }
}
