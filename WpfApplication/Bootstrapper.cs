using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using AvalonDockAdapter.Views;
using AvalonDockAdapter;
using Microsoft.Practices.Prism.Regions;
using WpfApplication.Views;
using WpfApplication.ViewModels;

namespace WpfApplication
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            // TODO: read configuration 
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var moduleCatalog = (ModuleCatalog)this.ModuleCatalog;

            // TODO: add dynamically from directory traverse
            //moduleCatalog.AddModule(typeof(AvalonDockAdapter.AvalonDockAdapterModule));
            moduleCatalog.AddModule(typeof(Sample.SampleModule));
            moduleCatalog.AddModule(typeof(WindowsExplorerish.WindowsExplorerishModule));
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            //this.InitializeDefaultViews();
        }

        protected override DependencyObject CreateShell()
        {
            var shell = this.Container.Resolve<Shell>();
            return shell;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();

            //mappings.RegisterMapping(AvalonDockRegionAdapter.AdapteeType, this.Container.Resolve<AvalonDockRegionAdapter>());

            return mappings;
        }

        private void InitializeDefaultViews()
        {
            var regionManager = this.Container.Resolve<IRegionManager>();
            if (regionManager.Regions["MainPane"].Views.Count() == 0)
            {
                regionManager.AddToRegion("MainPane", this.Container.Resolve<PrismMainContent>());
            }
        }
    }
}
