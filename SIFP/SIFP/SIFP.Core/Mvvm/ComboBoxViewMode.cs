using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SIFP.Core.Mvvm
{
    public class ComboBoxViewMode<T> : ViewModelBase
    {
        public string Description { get; set; }
        public T SelectedModel { get; set; }
        private Visibility isShow = Visibility.Visible;
        public Visibility IsShow
        {
            get { return isShow; }
            set
            {
                isShow = value;
                RaisePropertyChanged();
            }
        }
    }
}
