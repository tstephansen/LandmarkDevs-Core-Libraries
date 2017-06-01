using System;
using System.Collections.Generic;
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
        /// <param name="client">The client.</param>
        /// <exception cref="System.ArgumentNullException">client</exception>
        public TelemetryTracker(IHockeyClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
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
            _client.TrackEvent("Invalid Action Event", new Dictionary<string, string>
            {
                {"User", user },
                {"Location", location },
                {"Summary", summary },
                {"Date",  dateStamp},
                {"Time", timeStamp }
            });
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
            _client.TrackEvent(actionName, new Dictionary<string, string>
            {
                {"User", user },
                {"Summary", actionSummary },
                {"Date",  dateStamp},
                {"Time", timeStamp }
            });
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
            _client.TrackEvent(actionName, actionProperties);
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
            _client.TrackException(ex, new Dictionary<string, string>
            {
                {"User", user },
                {"Exception Message", ex.Message.Trim() },
                {"Location", location },
                {"Date", dateStamp },
                {"Time", timeStamp }
            });
        }

        private readonly IHockeyClient _client;
    }
}