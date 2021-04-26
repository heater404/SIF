using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Services.Interfaces;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PasswordDialog.ViewModels
{
    public class PasswordViewModel : RegionViewModelBase, IDialogAware
    {
        public DelegateCommand<PasswordBox> GetPasswordCmd { get; set; }
       
        public PasswordViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(regionManager, eventAggregator)
        {
            GetPasswordCmd = new DelegateCommand<PasswordBox>(GetPassword);
        }

        private void GetPassword(PasswordBox pwd)
        {
            if (pwd == null)//cancle
                RequestClose.Invoke(new DialogResult(ButtonResult.Cancel));//close dialog and callback
            else//confirm the password
            {
                if (pwd.Password != "4Ferliu10")
                {
                    Warning = "wrong password";
                }
                else//密码正确
                {
                    RequestClose.Invoke(new DialogResult(ButtonResult.Yes));//close dialog and callback
                }
            }
        }

        private string warning;
        public string Warning
        {
            get { return warning; }
            set { warning = value; RaisePropertyChanged(); }
        }

        public string Title => "Password";

        public event Action<IDialogResult> RequestClose;

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
