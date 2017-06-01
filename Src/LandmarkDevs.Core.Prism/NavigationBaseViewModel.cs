using LandmarkDevs.Core.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using NLog;
using Prism.Regions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LandmarkDevs.Core.Prism
{
    /// <summary>
    /// Class NavigationBaseViewModel.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    /// <seealso cref="INavigationAware" />
    public class NavigationBaseViewModel : BaseViewModel, INavigationAware
    {
        #region INavigationAware
        /// <summary>
        /// Called to determine if this instance can handle the navigation request.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns><see langword="true" /> if this instance accepts the navigation request; otherwise, <see langword="false" />.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext == null)
                throw new ArgumentNullException(nameof(navigationContext));
            return true;
        }

        /// <summary>
        /// Called when the implementer is being navigated away from.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (navigationContext == null)
                throw new ArgumentNullException(nameof(navigationContext));
            var journalEntry = new RegionNavigationJournalEntry { Uri = navigationContext.Uri };
            NavigationJournal = NavigationJournal ?? ServiceLocator.Current.GetInstance<IRegionNavigationJournal>();
            NavigationJournal.RecordNavigation(journalEntry);
        }

        /// <summary>
        /// Called when the implementer has been navigated to.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext == null)
                throw new ArgumentNullException(nameof(navigationContext));
            NavigationJournal = navigationContext.NavigationService.Journal;
        }

        #endregion INavigationAware

        #region Region Navigation
        /// <summary>
        /// Navigates the specified view name.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        public virtual void Navigate(string viewName)
        {
            var navModel = new NavModel(viewName);
            Navigate(navModel);
        }

        /// <summary>
        /// Navigates the specified view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        public virtual void Navigate(INavModel navModel)
        {
            Logger.Log(LogLevel.Info, $"Navigating to {navModel.ViewName}.");
            try
            {
                RegionManager.RequestNavigate(Regions.MainRegion, navModel.ViewName, NavigationCallback);
            }
            catch (Exception ex)
            {
                ErrorTracker.LogError(new ErrorModel(ex, Environment.UserName, "NavigationBaseViewModel.Navigate"));
                Logger.Log(LogLevel.Error, "Failed to navigate to view. Location: NavigationBaseViewModel.Navigate");
                Logger.Log(LogLevel.Error, ex, ex.Message.Trim(), ex.StackTrace);
            }
        }

        /// <summary>
        /// Navigates the close.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        public virtual void NavigateClose(string viewName)
        {
            var navModel = new NavModel(viewName);
            NavigateClose(navModel);
        }

        /// <summary>
        /// Navigates to the specified view and closes the current view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        public virtual void NavigateClose(INavModel navModel)
        {
            Logger.Log(LogLevel.Info, $"Navigating to {navModel.ViewName}.");
            try
            {
                RegionManager.RequestNavigate(Regions.MainRegion, navModel.ViewName, NavigationCallback);
                Close(navModel);
            }
            catch (Exception ex)
            {
                ErrorTracker.LogError(new ErrorModel(ex, Environment.UserName,
                    "NavigationBaseViewModel.NavigateClose"));
                Logger.Log(LogLevel.Error, "Failed to navigate to view. Location: NavigationBaseViewModel.NavigateClose");
                Logger.Log(LogLevel.Error, ex, ex.Message.Trim(), ex.StackTrace);
            }
        }

        /// <summary>
        /// Navigates to the specified view with parameters.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        public virtual void NavigateWith(INavModel navModel)
        {
            var message = $"Navigating to {navModel.ViewName} with the following parameters:\n";
            message = navModel.NavigationParameters.Aggregate(message, (current, o) => current + $"{o.Key} - {o.Value}\n");
            Logger.Log(LogLevel.Info, message);
            try
            {
                RegionManager.RequestNavigate(Regions.MainRegion, navModel.ViewName, NavigationCallback,
                    navModel.NavigationParameters);
            }
            catch (Exception ex)
            {
                ErrorTracker.LogError(new ErrorModel(ex, Environment.UserName,
                    "NavigationBaseViewModel.NavigateWith"));
                Logger.Log(LogLevel.Error, "Failed to navigate to view. Location: NavigationBaseViewModel.NavigateWith");
                Logger.Log(LogLevel.Error, ex, ex.Message.Trim(), ex.StackTrace);
            }
        }

        /// <summary>
        /// Navigates to the specified view with parameters and closes the current view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        public virtual void NavigateWithClose(INavModel navModel)
        {
            var message = $"Navigating to {navModel.ViewName} with the following parameters:\n";
            message = navModel.NavigationParameters.Aggregate(message, (current, o) => current + $"{o.Key} - {o.Value}\n");
            Logger.Log(LogLevel.Info, message);
            try
            {
                RegionManager.RequestNavigate(Regions.MainRegion, navModel.ViewName, NavigationCallback,
                    navModel.NavigationParameters);
                var vm = navModel.ViewModel as NavigationBaseViewModel;
                if (vm == null)
                {
                    Debug.WriteLine("VM NULL!!");
                    return;
                }
                navModel.ViewId = vm.ViewId;
                Close(navModel);
            }
            catch (Exception ex)
            {
                ErrorTracker.LogError(new ErrorModel(ex, Environment.UserName,
                    "NavigationBaseViewModel.NavigateWithClose"));
                Logger.Log(LogLevel.Error, "Failed to navigate to view. Location: NavigationBaseViewModel.NavigateWithClose");
                Logger.Log(LogLevel.Error, ex, ex.Message.Trim(), ex.StackTrace);
            }
        }

        /// <summary>
        /// Closes the specified view.
        /// </summary>
        /// <param name="navModel">The nav model.</param>
        public virtual void Close(INavModel navModel)
        {
            var region = RegionManager.Regions[Regions.MainRegion];
            for (var i = 0; i < region.Views.Count(); i++)
            {
                var v = (UserControl)region.Views.ElementAt(i);
                var d = v.DataContext;
                if (d.GetType() != navModel.ViewModel.GetType())
                    continue;
                if (((NavigationBaseViewModel)d).ViewId != navModel.ViewId)
                    continue;
                try
                {
                    var fe = (FrameworkElement)v;
                    region.Remove(fe);
                }
                catch (Exception ex)
                {
                    ErrorTracker.LogError(new ErrorModel(ex, Environment.UserName, "NavigationBaseViewModel.Close"));
                    Logger.Log(LogLevel.Error, ex.Message.Trim(), ex.Data);
                }
            }
        }

        /// <summary>
        /// Closes the specified view.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="viewId">The view identifier.</param>
        public virtual void Close(dynamic viewModel, string viewId)
        {
            var region = RegionManager.Regions[Regions.MainRegion];
            for (var i = 0; i < region.Views.Count(); i++)
            {
                var v = (UserControl)region.Views.ElementAt(i);
                var d = v?.DataContext;
                if (d == null)
                    continue;
                if (d.GetType() != viewModel.GetType())
                    continue;
                if (((NavigationBaseViewModel)d).ViewId != viewId)
                    continue;
                try
                {
                    var fe = (FrameworkElement)v;
                    region.Remove(fe);
                }
                catch (Exception ex)
                {
                    ErrorTracker.LogError(new ErrorModel(ex, Environment.UserName, "NavigationBaseViewModel.Close"));
                    Logger.Log(LogLevel.Error, ex.Message.Trim(), ex.Data);
                }
            }
        }

        /// <summary>
        /// Called when the navigation action has completed.
        /// </summary>
        /// <param name="result">The result.</param>
        public virtual void NavigationCallback(NavigationResult result)
        {
            if (result.Result == false)
                OnNavigationFailed(result.Error);
            else
            {
                Logger.Log(LogLevel.Info, "Navigation Complete");
            }
        }

        /// <summary>
        /// Called when navigation fails.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public virtual void OnNavigationFailed(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            if (ex.GetBaseException().Message == "Request for principal permission failed.")
                Application.Current.Dispatcher.Invoke(
                    () =>
                    {
                        MessageBox.Show("Authorization Error",
                            "You are not authorized to access this part of the system. If you believe this is in error please contact the administrator for assistance.");
                    });
            Logger.Log(LogLevel.Error, "Region Navigation Failed!");
            Logger.Log(LogLevel.Error, ex, ex.Message.Trim(), ex.StackTrace);
        }

        #endregion Region Navigation

        #region Variables
        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        /// <value>The navigation service.</value>
        public virtual IRegionNavigationService NavigationService { get; } = ServiceLocator.Current.GetInstance<IRegionNavigationService>();

        /// <summary>
        /// Gets or sets the navigation journal.
        /// </summary>
        /// <value>The navigation journal.</value>
        public virtual IRegionNavigationJournal NavigationJournal { get; set; }

        /// <summary>
        /// Gets the region manager.
        /// </summary>
        /// <value>The region manager.</value>
        public virtual IRegionManager RegionManager { get; } = ServiceLocator.Current.GetInstance<IRegionManager>();

        /// <summary>
        /// Gets the error tracker.
        /// </summary>
        /// <value>The error tracker.</value>
        public virtual IErrorTracker ErrorTracker { get; } = ServiceLocator.Current.GetInstance<IErrorTracker>();

        #endregion
    }
}