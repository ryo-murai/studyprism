using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using WindowsExplorerish.Views;

namespace WindowsExplorerish
{
    public class WindowsExplorerishModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public WindowsExplorerishModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.container.RegisterType<object, FolderTreeView>("FolderTreeView");
            this.container.RegisterType<object, FileListView>("FileListView");

            this.regionManager.RequestNavigate("LeftRegion", "FolderTreeView");
            this.regionManager.RequestNavigate("MainRegion", "FileListView");

            var ribbonRegion = this.regionManager.Regions["RibbonRegion"];
            var homeRibbonTab = this.container.Resolve<HomeRibbonTab>();
            ribbonRegion.Add(homeRibbonTab);
            ribbonRegion.Activate(homeRibbonTab);
        }
    }
}
