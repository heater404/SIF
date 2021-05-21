using Prism.Commands;
using Prism.Mvvm;
using Serilog;
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
                try
                {
                    Clipboard.SetText(msg);
                }
                catch (System.Exception ex)
                {
                    Log.Logger.Error(ex,"Clipboard Error");
                }
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
