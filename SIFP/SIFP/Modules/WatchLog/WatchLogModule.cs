using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SIFP.Core;
using WatchLog.Views;

namespace WatchLog
{
    public class WatchLogModule : IModule
    {
        private IRegionManager regionManager;
        public WatchLogModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion(RegionNames.WatchLogRegion, typeof(WatchLogView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}