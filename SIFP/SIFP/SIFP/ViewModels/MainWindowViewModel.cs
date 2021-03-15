using Prism.Commands;
using Prism.Mvvm;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace SIFP.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string title = "SI View";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private ObservableCollection<WatchLog> watchLogs=new ObservableCollection<WatchLog>();
        public ObservableCollection<WatchLog> WatchLogs 
        {
            get { return watchLogs; }
            set { watchLogs = value;RaisePropertyChanged(); }
        }

        public DelegateCommand ClearWatchLogsCmd { get; private set; }
        public DelegateCommand SaveWatchLogsCmd { get; private set; }

        public MainWindowViewModel()
        {
            ClearWatchLogsCmd = new DelegateCommand(ClearWatchLogs);
            SaveWatchLogsCmd = new DelegateCommand(SaveWatchLogs);

            AddWatchLog(new WatchLog("AAAACC", WatchLogLevel.Error));
            AddWatchLog(new WatchLog("AEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEECCCDWWWWWWWWWWWWWWWWWWWWWWWWWWWWBBBBFFFFFFFFFF", WatchLogLevel.Warning));
            AddWatchLog(new WatchLog("CC", WatchLogLevel.Info));
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
                    foreach (var log in watchLogs)
                    {
                        sw.WriteLine(log.ToString());
                    }
                }
            }
        }

        private void ClearWatchLogs()
        {
            App.Current.Dispatcher.Invoke(() =>
            WatchLogs.Clear()
            ); ;
        }

        public void AddWatchLog(WatchLog log)
        {
            App.Current.Dispatcher.Invoke(() =>
            WatchLogs.Add(log)
            );
        }
    }
}
