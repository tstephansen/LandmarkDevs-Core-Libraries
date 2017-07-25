using System;
using System.ComponentModel;
using LandmarkDevs.Core.Infrastructure;
using NLog;
using Prism.Events;
using Prism.Regions;
namespace LandmarkDevs.Core.Prism
{
    /// <summary>
    /// Interface INavigationBaseViewModel
    /// </summary>
    /// <seealso cref="INavigationAware" />
    public interface INavigationBaseViewModel : INavigationAware, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Navigates the specified view name.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        void Navigate(string viewName);

        /// <summary>
        /// Navigates the specified view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        void Navigate(INavModel navModel);

        /// <summary>
        /// Navigates the close.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        void NavigateClose(string viewName);

        /// <summary>
        /// Navigates to the specified view and closes the current view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        void NavigateClose(INavModel navModel);

        /// <summary>
        /// Navigates to the specified view with parameters.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        void NavigateWith(INavModel navModel);

        /// <summary>
        /// Navigates to the specified view with parameters and closes the current view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        void NavigateWithClose(INavModel navModel);

        /// <summary>
        /// Closes the specified view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        void Close(INavModel navModel);

        /// <summary>
        /// Closes the specified view.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="viewId">The view identifier.</param>
        void Close(dynamic viewModel, string viewId);

        /// <summary>
        /// Called when the navigation action has completed.
        /// </summary>
        /// <param name="result">The result.</param>
        void NavigationCallback(NavigationResult result);

        /// <summary>
        /// Called when navigation fails.
        /// </summary>
        /// <param name="ex">The ex.</param>
        void OnNavigationFailed(Exception ex);

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        /// <value>The navigation service.</value>
        IRegionNavigationService NavigationService { get; }

        /// <summary>
        /// Gets or sets the navigation journal.
        /// </summary>
        /// <value>The navigation journal.</value>
        IRegionNavigationJournal NavigationJournal { get; set; }

        /// <summary>
        /// Gets the region manager.
        /// </summary>
        /// <value>The region manager.</value>
        IRegionManager RegionManager { get; }

        /// <summary>
        /// Gets the error tracker.
        /// </summary>
        /// <value>The error tracker.</value>
        IErrorTracker ErrorTracker { get; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the view identifier.
        /// </summary>
        /// <value>The view identifier.</value>
        string ViewId { get; set; }

        /// <summary>
        /// Gets or sets the visual state.
        /// </summary>
        /// <value>The visual state.</value>
        string VisualState { get; set; }

        /// <summary>
        /// Gets the event aggregator.
        /// </summary>
        /// <value>The event aggregator.</value>
        IEventAggregator EventAggregator { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        ILogger Logger { get; }
    }
}