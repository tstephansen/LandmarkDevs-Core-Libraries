using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.HockeyApp;

namespace LandmarkDevs.Core.Telemetry
{
    /// <summary>
    /// Interface ITelemetryTracker.
    /// Used to track telemetry data.
    /// </summary>
    public interface ITelemetryTracker
    {
        /// <summary>
        /// Tracks the invalid action. This is called when the user tries to perform an action that isn't allowed.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="user">The user.</param>
        void TrackInvalidAction(string location, string summary, string user);

        /// <summary>
        /// Tracks the action. This is called when the user performs an action that is tracked.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="actionSummary">The action summary.</param>
        /// <param name="user">The user.</param>
        void TrackAction(string actionName, string actionSummary, string user);

        /// <summary>
        /// Tracks the action. This is called when the user performs an action that is tracked.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="actionSummary">The action summary.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="user">The user.</param>
        void TrackAction(string actionName, string actionSummary, Dictionary<string, string> properties, string user);

        /// <summary>
        /// Tracks the error. This is called when an exception is logged.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="location">The location.</param>
        /// <param name="user">The user.</param>
        void TrackError(Exception ex, string location, string user);

        /// <summary>
        /// Tracks the user's navigation through the application.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="properties">The properties.</param>
        void TrackNavigation(string viewName, TimeSpan duration, IDictionary<string, string> properties = null);

        /// <summary>
        /// Gets or sets the application client.
        /// </summary>
        /// <value>The application client.</value>
        TelemetryClient AppClient { get; set; }

        /// <summary>
        /// Gets or sets the hockey client.
        /// </summary>
        /// <value>The hockey client.</value>
        IHockeyClient HockeyClient { get; set; }
    }
}