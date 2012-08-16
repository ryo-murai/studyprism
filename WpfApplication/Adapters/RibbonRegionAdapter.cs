using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Windows.Controls.Ribbon;
using System.Collections.Specialized;

namespace WpfApplication.Adapters
{
    class RibbonRegionAdapter : RegionAdapterBase<Ribbon>
    {
        public RibbonRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, Ribbon regionTarget)
        {
            region.Views.CollectionChanged += delegate(Object sender, NotifyCollectionChangedEventArgs e)
            {
                this.OnViewsCollectionChanged(sender, e, region, regionTarget);
            };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        private void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, IRegion region, Ribbon regionTarget)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    int insertIndex = e.NewStartingIndex;
                    foreach (RibbonTab tab in e.NewItems)
                    {
                        regionTarget.Items.Insert(insertIndex++, tab);
                    }

                    regionTarget.SelectedIndex = 0;
                    break;

                case NotifyCollectionChangedAction.Remove:
                    // 
                    break;

                default:
                    break;
            }
        }
    }
}
