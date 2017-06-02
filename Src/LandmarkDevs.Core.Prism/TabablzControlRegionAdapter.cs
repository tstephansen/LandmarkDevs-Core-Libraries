using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using Dragablz;
using Prism.Regions;

namespace LandmarkDevs.Core.Prism
{
    /// <summary>
    ///     Class TabablzControlRegionAdapter.
    /// </summary>
    /// <seealso cref="TabablzControl" />
    public class TabablzControlRegionAdapter : RegionAdapterBase<TabablzControl>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TabablzControlRegionAdapter" /> class.
        /// </summary>
        /// <param name="regionBehaviorFactory">The factory used to create the region behaviors to attach to the created regions.</param>
        public TabablzControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {
        }

        /// <summary>
        ///     Template method to adapt the object to an <see cref="T:Prism.Regions.IRegion" />.
        /// </summary>
        /// <param name="region">The new region being used.</param>
        /// <param name="regionTarget">The object to adapt.</param>
        protected override void Adapt(IRegion region, TabablzControl regionTarget)
        {
            region.ActiveViews.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    default:
                        break;
                    case NotifyCollectionChangedAction.Add:
                        TabAdded(regionTarget, e);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        TabRemoved(regionTarget, e);
                        break;
                }
            };
        }

        private static void TabAdded(TabablzControl regionTarget, NotifyCollectionChangedEventArgs e)
        {
            foreach (object obj in e.NewItems)
            {
                var item = new TabItem();
                var vw = e.NewItems[0] as UserControl;
                item.Header = ((INavigationBaseViewModel) vw.DataContext).Title;
                item.Content = vw;
                regionTarget.Items.Insert(regionTarget.Items.Count, item);
                regionTarget.SelectedIndex = regionTarget.Items.Count - 1;
            }
        }

        private static void TabRemoved(TabablzControl regionTarget, NotifyCollectionChangedEventArgs e)
        {
            foreach (object obj in e.OldItems)
            {
                for (var i = 0; i < regionTarget.Items.Count; i++)
                {
                    var tab = (TabItem) regionTarget.Items[i];
                    if (tab.Content == e.OldItems[0])
                        regionTarget.Items.Remove(tab);
                }
                regionTarget.SelectedIndex = regionTarget.Items.Count - 1;
            }
        }

        /// <summary>
        ///     Template method to create a new instance of <see cref="T:Prism.Regions.IRegion" />
        ///     that will be used to adapt the object.
        /// </summary>
        /// <returns>A new instance of <see cref="T:Prism.Regions.IRegion" />.</returns>
        [ExcludeFromCodeCoverage]
        protected override IRegion CreateRegion() => new AllActiveRegion();
    }
}
