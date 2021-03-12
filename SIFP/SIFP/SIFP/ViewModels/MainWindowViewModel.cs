using Prism.Mvvm;

namespace SIFP.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "SIFP";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
