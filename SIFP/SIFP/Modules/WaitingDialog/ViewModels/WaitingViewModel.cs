using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaitingDialog.ViewModels
{
    public class WaitingViewModel : ViewModelBase, IDialogAware
    {
        public string Title { get; set; } = "Waiting";
        private IEventAggregator eventAggregator;
        public WaitingViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<CloseWaitingDialogEvent>().Subscribe(() => this.RequestClose?.Invoke(null),true);
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            this.eventAggregator.GetEvent<MainWindowEnableEvent>().Publish(true);
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            this.eventAggregator.GetEvent<MainWindowEnableEvent>().Publish(false);
        }
    }
}
