using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
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
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ToolViewModel(IRegionManager regionManager ):base(regionManager)
        {
            Message = "Tool";
        }
    }
}
