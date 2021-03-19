using ConfigCamera.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SIFP.Core;

namespace ConfigCamera
{
    public class ConfigCameraModule : IModule
    {
        private IRegionManager regionManager;
        public ConfigCameraModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion(RegionNames.LeftDrawerRegion, typeof(ConfigCameraView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}