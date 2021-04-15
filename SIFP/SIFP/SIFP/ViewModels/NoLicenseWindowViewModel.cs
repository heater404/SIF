using Prism.Mvvm;

namespace SIFP.ViewModels
{
    public class NoLicenseWindowViewModel : BindableBase
    {
        public NoLicenseWindowViewModel()
        {
            ComputerInfo = License.ComputerInfo.GetComputerInfo();
        }

        private string computerInfo;
        public string ComputerInfo
        {
            get { return computerInfo; }
            set { computerInfo = value; RaisePropertyChanged(); }
        }
    }
}
