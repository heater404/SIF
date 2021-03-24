using Prism.Events;
using Prism.Regions;
using SIFP.Core.Enums;
using SIFP.Core.Models;
using System;

namespace SIFP.Core.Mvvm
{
    public class RegionViewModelBase : ViewModelBase, IConfirmNavigationRequest
    {
        protected IRegionManager RegionManager { get; private set; }
        protected IEventAggregator EventAggregator { get; private set; }

        public RegionViewModelBase(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            RegionManager = regionManager;
            EventAggregator = eventAggregator;
        }

        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public virtual void PrintWatchLog(string log,LogLevel logLevel)
        {
            this.EventAggregator.GetEvent<WatchLogEvent>().Publish(new LogModel(log,logLevel));
        }

        public virtual void PrintNoticeLog(string log, LogLevel logLevel)
        {
            this.EventAggregator.GetEvent<NoticeLogEvent>().Publish(new LogModel(log, logLevel));
        }
    }
}
