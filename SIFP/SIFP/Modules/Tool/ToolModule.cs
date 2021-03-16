using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SIFP.Core;
using Tool.Views;

namespace Tool
{
    public class ToolModule : IModule
    {
        private IRegionManager regionManager;
        public ToolModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RequestNavigate(RegionNames.ToolRegion, typeof(ToolView).Name);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ToolView>(typeof(ToolView).Name);
        }
    }
}