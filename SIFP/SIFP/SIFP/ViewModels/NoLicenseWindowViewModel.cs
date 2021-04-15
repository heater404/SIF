using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace SIFP.ViewModels
{
    public class NoLicenseWindowViewModel : BindableBase
    {
        public NoLicenseWindowViewModel()
        {
            ComputerInfo = License.ComputerInfo.GetComputerInfo();
            CopyCmd = new DelegateCommand<string>(msg=>
            {
                Clipboard.SetText(msg);
            });
        }

        private string computerInfo;
        public string ComputerInfo
        {
            get { return computerInfo; }
            set { computerInfo = value; RaisePropertyChanged(); }
        }

        private string emailAddress= "daokuan.zhu@si-in.com";
        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value;RaisePropertyChanged(); }
        }

        public DelegateCommand<string> CopyCmd { get; set; }
    }
}
