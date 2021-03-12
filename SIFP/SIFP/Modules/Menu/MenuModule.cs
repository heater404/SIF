using Menu.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SIFP.Core;

namespace Menu
{
    public class MenuModule : IModule
    {
        private IRegionManager regionManager;
        public MenuModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RequestNavigate(RegionNames.MenuRegion, "MenuView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MenuView>("MenuView");
        }
    }
}