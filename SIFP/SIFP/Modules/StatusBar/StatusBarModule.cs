using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SIFP.Core;
using Services.Views;

namespace Services
{
    public class StatusBarModule : IModule
    {
        private IRegionManager regionManager;
        public StatusBarModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.StatusBarRegion, typeof(StatusBarView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}