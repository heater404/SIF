using Prism.Commands;
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
        private string _message;

        public event Action<IDialogResult> RequestClose;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public string Title { get; set; }

        public WaitingViewModel()
        {
            Message = "View A from your Prism Module";
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
