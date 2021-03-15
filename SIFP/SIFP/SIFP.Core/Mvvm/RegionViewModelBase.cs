using Prism.Events;
using Prism.Regions;
using System;

namespace SIFP.Core.Mvvm
{
    public class RegionViewModelBase : ViewModelBase, INavigationAware, IConfirmNavigationRequest
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
    }
}
