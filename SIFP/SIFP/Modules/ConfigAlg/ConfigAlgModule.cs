using ConfigAlg.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SIFP.Core;

namespace ConfigAlg
{
    public class ConfigAlgModule : IModule
    {
        private IRegionManager regionManager;
        public ConfigAlgModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //this.regionManager.RegisterViewWithRegion(RegionNames.LeftDrawerRegion, typeof(ConfigAlgView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}