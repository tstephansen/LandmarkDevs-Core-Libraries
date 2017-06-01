using System;
using System.Collections.Generic;

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
    }
}