using LandmarkDevs.Core.Infrastructure;
using Prism.Regions;
using System;

namespace LandmarkDevs.Core.Prism
{
    /// <summary>
    ///     Interface INavigationBaseViewModel
    /// </summary>
    /// <seealso cref="INavigationAware"/>
    public interface INavigationBaseViewModel : INavigationAware
    {
        /// <summary>
        ///     Closes the specified view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        void Close(INavModel navModel);

        /// <summary>
        ///     Closes the specified view.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="viewId">The view identifier.</param>
        void Close(dynamic viewModel, string viewId);

        /// <summary>
        ///     Called to determine if this instance can handle the navigation request.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>
        ///     <see langword="true"/> if this instance accepts the navigation request; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        bool IsNavigationTarget(NavigationContext navigationContext);

        /// <summary>
        ///     Navigates the specified view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        void Navigate(INavModel navModel);

        /// <summary>
        ///     Navigates the specified view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        void Navigate(string viewName);

        /// <summary>
        ///     Navigates to the specified view and closes the current view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        void NavigateClose(INavModel navModel);

        /// <summary>
        ///     Navigates to the specified view and closes the current view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        void NavigateClose(string viewName);

        /// <summary>
        ///     Navigates to the specified view with parameters.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        void NavigateWith(INavModel navModel);

        /// <summary>
        ///     Navigates to the specified view with parameters and closes the current view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        void NavigateWithClose(INavModel navModel);

        /// <summary>
        ///     Called when the navigation action has completed.
        /// </summary>
        /// <param name="result">The result.</param>
        void NavigationCallback(NavigationResult result);

        /// <summary>
        ///     Called when navigation fails.
        /// </summary>
        /// <param name="ex">The ex.</param>
        void OnNavigationFailed(Exception ex);

        /// <summary>
        ///     Gets or sets the error tracker.
        /// </summary>
        /// <value>The error tracker.</value>
        IErrorTracker ErrorTracker { get; set; }
        /// <summary>
        ///     Gets or sets a value indicating whether telemetry is disabled for this instance.
        /// </summary>
        /// <value><c>true</c> if telemetry is disabled for this instance; otherwise, <c>false</c>.</value>
        bool IsTelemetryDisabled { get; set; }
        /// <summary>
        ///     Gets or sets the navigation journal.
        /// </summary>
        /// <value>The navigation journal.</value>
        IRegionNavigationJournal NavigationJournal { get; set; }
        /// <summary>
        ///     Gets or sets the navigation service.
        /// </summary>
        /// <value>The navigation service.</value>
        IRegionNavigationService NavigationService { get; set; }
        /// <summary>
        ///     Gets or sets the page timer.
        /// </summary>
        /// <value>The page timer.</value>
        System.Diagnostics.Stopwatch PageTimer { get; set; }
        /// <summary>
        ///     Gets or sets the region manager.
        /// </summary>
        /// <value>The region manager.</value>
        IRegionManager RegionManager { get; set; }
        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }
        /// <summary>
        ///     Gets or sets the view identifier.
        /// </summary>
        /// <value>The view identifier.</value>
        string ViewId { get; set; }
        /// <summary>
        ///     Gets or sets the visual state.
        /// </summary>
        /// <value>The visual state.</value>
        string VisualState { get; set; }
    }
}