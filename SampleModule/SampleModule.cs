using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Sample.Views;

namespace Sample
{
    public class SampleModule : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;

        public SampleModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        public void Initialize()
        {
            this.regionManager.AddToRegion("RibbonRegion", this.container.Resolve<SampleRibbonTab>());
        }
    }
}
