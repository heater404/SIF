using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Serilog;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using SIFP.Core.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace SIFP.ViewModels
{
    public class MainWindowViewModel : RegionViewModelBase
    {
        private string title = "SI View";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {

        }
    }
}
