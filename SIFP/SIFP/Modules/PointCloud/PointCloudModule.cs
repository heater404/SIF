using PointCloud.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SIFP.Core;

namespace PointCloud
{
    public class PointCloudModule : IModule
    {
        private IRegionManager regionManager;
        public PointCloudModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(PointCloudView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}