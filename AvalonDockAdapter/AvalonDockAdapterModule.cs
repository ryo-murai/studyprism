using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using AvalonDockAdapter.Views;

namespace AvalonDockAdapter
{
    public class AvalonDockAdapterModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly RegionAdapterMappings mappings;

        public AvalonDockAdapterModule(IUnityContainer container, RegionAdapterMappings mappings)
        {
            this.container = container;
            this.mappings = mappings;
        }

        public void Initialize()
        {
            this.mappings.RegisterMapping(AvalonDockRegionAdapter.AdapteeType, this.container.Resolve<AvalonDockRegionAdapter>());
            var regionManager = this.container.Resolve<IRegionManager>();
            regionManager.AddToRegion("MainPane", this.container.Resolve<AvalonDockMainContent>());
        }
    }
}
