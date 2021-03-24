using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using SIFP.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIFP.Core.Models;
using System.IO;
using SIFP.Core.Enums;
using System.Windows;

namespace WatchLog.ViewModels
{
    public class WatchLogViewModel : RegionViewModelBase
    {
        private ObservableCollection<LogModel> watchLogs = new ObservableCollection<LogModel>();
        public ObservableCollection<LogModel> WatchLogs
        {
            get { return WatchLogs1; }
            set { WatchLogs1 = value; RaisePropertyChanged(); }
        }

        public DelegateCommand ClearWatchLogsCmd { get; private set; }
        public DelegateCommand SaveWatchLogsCmd { get; private set; }
        public ObservableCollection<LogModel> WatchLogs1 { get => watchLogs; set => watchLogs = value; }

        public WatchLogViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            ClearWatchLogsCmd = new DelegateCommand(ClearWatchLogs);
            SaveWatchLogsCmd = new DelegateCommand(SaveWatchLogs);
            this.EventAggregator.GetEvent<WatchLogEvent>().Subscribe(AddWatchLog);
        }

        private void SaveWatchLogs()
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "text files|*.txt"
            };
            if (sfd.ShowDialog().Value)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    foreach (var log in WatchLogs1)
                    {
                        sw.WriteLine(log.ToString());
                    }
                }
            }
        }

        private void ClearWatchLogs()
        {
            WatchLogs.Clear();
        }

        private void AddWatchLog(LogModel log)
        {
            Application.Current.Dispatcher.Invoke(() => WatchLogs.Add(log));
        }
    }
}
