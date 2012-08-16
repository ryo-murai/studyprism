using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using AvalonDock;
using AvalonDockAdapter.ViewModels;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;

namespace AvalonDockAdapter
{
    public class AvalonDockRegionAdapter : RegionAdapterBase<Pane>
    {
        public static Type AdapteeType = typeof(Pane);

        public AvalonDockRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, Pane regionTarget)
        {
            Debug.Assert(region is ActivationAwareRegion, "check implementation of CreateRegion() method");

            ((ActivationAwareRegion)region).ActivationChangedEvent += delegate(object sender, ActivationChangedEventArgs e)
            {
                this.OnRegionActivationChangedEvent(sender, e, region, regionTarget);
            };

            region.Views.CollectionChanged += delegate(Object sender, NotifyCollectionChangedEventArgs e)
            {
                this.OnViewsCollectionChanged(sender, e, region, regionTarget);
            };
        }

        protected override IRegion CreateRegion()
        {
            return new ActivationAwareRegion();
        }

        private void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, IRegion region, Pane regionTarget)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //Add content panes for each associated view.
                int insertIndex = e.NewStartingIndex;
                foreach (object item in e.NewItems)
                {
                    UIElement view = item as UIElement;

                    if (view != null)
                    {
                        ManagedContent newContent = this.CreateContent(regionTarget);
                        newContent.Content = item;
                        newContent.IsCloseable = true;

                        // binding content title
                        if (view is FrameworkElement)
                        {
                            Binding binding = new Binding(BoundProperyNameToTitle);
                            binding.Source = ((FrameworkElement)view).DataContext;
                            newContent.SetBinding(ManagedContent.TitleProperty, binding);
                        }

                        //When contentPane is closed remove the associated region
                        newContent.Closed += (contentPaneSender, args) =>
                        {
                            region.Remove(newContent.Content);
                        };
                        regionTarget.Items.Insert(insertIndex++, newContent);
                        newContent.Activate();
                    }
                }
            }
            else
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    // TODO: handle removing view from region
                }
            }
        }

        private ManagedContent CreateContent(Pane regionTarget)
        {
            if (regionTarget is DocumentPane)
            {
                return new DocumentContent();
            }
            else
            {
                return new DockableContent
                {
                    HideOnClose = false
                };
            }
        }

        private void OnRegionActivationChangedEvent(object sender, ActivationChangedEventArgs e, IRegion region, Pane regionTarget)
        {
            var contentToActivate = regionTarget.Items.OfType<ManagedContent>()
                                        .Where(c => c.Content == e.Activated).FirstOrDefault();
            regionTarget.SelectedItem = contentToActivate;
        }

        internal class ActivationChangedEventArgs : EventArgs
        {
            public ActivationChangedEventArgs(object activatedView)
                : base()
            {
                this.Activated = activatedView;
            }

            public object Activated { get; private set; }
        }

        internal delegate void ActivationChangedEventHandler(object sender, ActivationChangedEventArgs e);

        internal class ActivationAwareRegion : Region
        {
            public event ActivationChangedEventHandler ActivationChangedEvent = delegate { };

            public override void Activate(object view)
            {
                base.Activate(view);

                this.ActivationChangedEvent(this, new ActivationChangedEventArgs(view));
            }
        }


        #region Typesafe property name definition

        private static readonly string BoundProperyNameToTitle = PropertySupport.ExtractPropertyName(() => new ContentModel().ContentTitle);

        private class ContentModel : IContentModel
        {
            public string ContentTitle
            {
                get { throw new NotImplementedException(); }
            }
        }

        #endregion
    }
}
