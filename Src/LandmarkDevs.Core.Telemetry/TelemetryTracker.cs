using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.HockeyApp;

namespace LandmarkDevs.Core.Telemetry
{
    /// <summary>
    /// Class TelemetryTracker.
    /// Used to track telemetry data.
    /// </summary>
    /// <seealso cref="ITelemetryTracker" />
    public class TelemetryTracker : ITelemetryTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryTracker"/> class.
        /// </summary>
        /// <param name="hockeyClient">The hockey client.</param>
        /// <param name="appClient">The application client.</param>
        public TelemetryTracker(IHockeyClient hockeyClient, TelemetryClient appClient)
        {
            HockeyClient = hockeyClient;
            AppClient = appClient;
        }
        /// <summary>
        /// Tracks the invalid action. This is called when the user tries to perform an action that isn't allowed.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="user">The user.</param>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public virtual void TrackInvalidAction(string location, string summary, string user)
        {
            var dateStamp = DateTime.Today.ToShortDateString();
            var timeStamp = DateTime.Now.ToLongTimeString();
            var eventProperties = new Dictionary<string, string>
            {
                {"User", user },
                {"Location", location },
                {"Summary", summary },
                {"Date",  dateStamp},
                {"Time", timeStamp }
            };
            if(HockeyClient != null) HockeyClient.TrackEvent("Invalid Action Event", eventProperties);
            if (AppClient != null) AppClient.TrackEvent("Invalid Action Event", eventProperties);
        }

        /// <summary>
        /// Tracks the action. This is called when the user performs an action that is tracked.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="actionSummary">The action summary.</param>
        /// <param name="user">The user.</param>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public virtual void TrackAction(string actionName, string actionSummary, string user)
        {
            var dateStamp = DateTime.Today.ToShortDateString();
            var timeStamp = DateTime.Now.ToLongTimeString();
            var actionProperties = new Dictionary<string, string>
            {
                {"User", user },
                {"Summary", actionSummary },
                {"Date",  dateStamp},
                {"Time", timeStamp }
            };
            if(HockeyClient != null) HockeyClient.TrackEvent(actionName, actionProperties);
            if(AppClient != null) AppClient.TrackEvent(actionName, actionProperties);
        }

        /// <summary>
        /// Tracks the action. This is called when the user performs an action that is tracked.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="actionSummary">The action summary.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="user">The user.</param>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public virtual void TrackAction(string actionName, string actionSummary, Dictionary<string, string> properties, string user)
        {
            var dateStamp = DateTime.Today.ToShortDateString();
            var timeStamp = DateTime.Now.ToLongTimeString();
            var actionProperties = new Dictionary<string, string>
            {
                {"User", user },
                {"Summary", actionSummary },
                {"Date",  dateStamp},
                {"Time", timeStamp }
            };
            foreach (var o in properties)
            {
                actionProperties.Add(o.Key, o.Value);
            }
            if (HockeyClient != null) HockeyClient.TrackEvent(actionName, actionProperties);
            if (AppClient != null) AppClient.TrackEvent(actionName, actionProperties);
        }

        /// <summary>
        /// Tracks the error. This is called when an exception is logged.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="location">The location.</param>
        /// <param name="user">The user.</param>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public virtual void TrackError(Exception ex, string location, string user)
        {
            var dateStamp = DateTime.Today.ToShortDateString();
            var timeStamp = DateTime.Now.ToLongTimeString();
            var eventProperties = new Dictionary<string, string>
            {
                {"User", user },
                {"Exception Message", ex.Message.Trim() },
                {"Location", location },
                {"Date", dateStamp },
                {"Time", timeStamp }
            };
            if(HockeyClient != null) HockeyClient.TrackException(ex, eventProperties);
            if (AppClient != null)
            {
                var appEx = new ExceptionTelemetry(ex);
                AppClient.TrackException(appEx);
                AppClient.TrackTrace(ex.StackTrace);
                AppClient.Flush();
            }
        }

        /// <summary>
        /// Tracks the user's navigation through the application.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="properties">The properties.</param>
        public virtual void TrackNavigation(string viewName, TimeSpan duration, IDictionary<string, string> properties = null)
        {
            if (AppClient == null) return;
            var telemetryData = new PageViewTelemetry
            {
                Name = viewName,
                Duration = duration,
                Timestamp = DateTimeOffset.Now
            };
            if(properties != null)
            {
                if(properties.Count > 0)
                {
                    foreach (var o in properties)
                    {
                        telemetryData.Properties.Add(o.Key, o.Value);
                    }
                }
            }
            AppClient.TrackPageView(telemetryData);
        }

        public virtual TelemetryClient AppClient { get; set; }

        public virtual IHockeyClient HockeyClient { get; set; }
    }
}