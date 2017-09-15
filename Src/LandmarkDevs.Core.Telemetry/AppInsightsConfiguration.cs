using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace LandmarkDevs.Core.Telemetry
{
    /// <summary>
    /// Configures Application Insights Telemetry.
    /// Code was mostly taken from https://www.meziantou.net/2017/03/29/use-application-insights-in-a-desktop-application
    /// </summary>
    public static class AppInsightsConfiguration
    {
        public static TelemetryClient ConfigureApplicationInsights(string instrumentationKey, bool useLocal = false, string storageFolder = null)
        {
            var config = new TelemetryConfiguration
            {
                InstrumentationKey = instrumentationKey
            };
            if(useLocal)
            {
                var channel = new Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ServerTelemetryChannel
                {
                    StorageFolder = storageFolder
                };
                channel.Initialize(TelemetryConfiguration.Active);
                config.TelemetryChannel = channel;
            }
            else
            {
                config.TelemetryChannel = new Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ServerTelemetryChannel();
            }
            //config.TelemetryChannel = new Microsoft.ApplicationInsights.Channel.InMemoryChannel(); // Default channel
            config.TelemetryChannel.DeveloperMode = Debugger.IsAttached;
#if DEBUG
            config.TelemetryChannel.DeveloperMode = true;
#endif
            var client = new TelemetryClient(config);
            client.Context.Component.Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            client.Context.Session.Id = Guid.NewGuid().ToString();
            client.Context.User.Id = (Environment.UserName + Environment.MachineName).GetHashCode().ToString();
            client.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
            TelemetryClient = client;
            return TelemetryClient;
        }

        /// <summary>
        /// Sets the authenticated user.
        /// </summary>
        /// <param name="user">The user.</param>
        public static void SetAuthenticatedUser(string user)
        {
            TelemetryClient.Context.User.AuthenticatedUserId = user;
        }

        /// <summary>
        /// Sets the user.
        /// </summary>
        /// <param name="user"></param>
        public static void SetUser(string user)
        {
            TelemetryClient.Context.User.Id = user;
        }

        /// <summary>
        /// Flushes the telemetry data.
        /// </summary>
        public static void Flush()
        {
            TelemetryClient.Flush();
        }

        /// <summary>
        /// Gets or sets the telemetry client.
        /// </summary>
        /// <value>The telemetry client.</value>
        public static TelemetryClient TelemetryClient { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AppInsightsConfiguration"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public static bool Enabled { get; set; }
    }
}
