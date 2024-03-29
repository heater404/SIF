﻿using Menu.Views;
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
            regionManager.RegisterViewWithRegion(RegionNames.MenuRegion, typeof(MenuView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}