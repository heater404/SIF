using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Mvvm
{

    public class DialogViewModelBase : ViewModelBase, IDialogAware
    {
        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
            
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
