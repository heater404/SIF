using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDialog.ViewModels
{
    public class NotificationViewModel : RegionViewModelBase, IDialogAware
    {
        private string notice;

        public event Action<IDialogResult> RequestClose;

        public string Notice
        {
            get { return notice; }
            set { SetProperty(ref notice, value); }
        }

        public string Title { get; set; }

        public DelegateCommand YesCmd { get; set; }
        public DelegateCommand NoCmd { get; set; }
        public NotificationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            YesCmd = new DelegateCommand(Yes);
            NoCmd = new DelegateCommand(No);
        }

        private void No()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private void Yes()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Yes));
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
            Notice= parameters?.GetValue<string>("notice");
        }
    }
}
