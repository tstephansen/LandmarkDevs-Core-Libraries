using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace LandmarkDevs.Core.Telemetry
{
    /// <summary>
    /// Configures Application Insights Telemetry.
    /// Code was mostly taken from https://www.meziantou.net/2017/03/29/use-application-insights-in-a-desktop-application
    /// Some of the context information methods were taken from https://github.com/NinetailLabs/VaraniumSharp.Initiator
    /// </summary>
    public class AppInsightsConfiguration
    {
        /// <summary>
        /// Configures Application Insights using the telemetry client that is passed into the constructor.
        /// </summary>
        /// <param name="client">The telemetry client.</param>
        public static void ConfigureApplicationInsights(TelemetryClient client)
        {
            TelemetryClient = client;
        }

        /// <summary>
        /// Configures Application Insights using the given instrumentation key.
        /// </summary>
        /// <param name="instrumentationKey">The .</param>
        /// <param name="useLocal">If <c>true</c> telemetry data will be stored locally if the server endpoint is unavailable.</param>
        /// <param name="storageFolder">The folder where offline telemetry data will be stored.</param>
        /// <returns>TelemetryClient</returns>
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
            client.Context.User.Id = GetCurrentUserInformation();
            client.Context.Device.OperatingSystem = GetWindowsFriendlyName();
            client.Context.Device.Model = GetDeviceModel();
            client.Context.Device.OemName = GetDeviceManufacturer();
            TelemetryClient = client;
            return TelemetryClient;
        }

        public static string GetCurrentUserInformation()
        {
            string domainName = string.Empty;
            try
            {
                domainName = System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain().Name;
                if(string.IsNullOrEmpty(domainName))
                {
                    return $"{Environment.UserDomainName} - {Environment.UserName} - {Environment.MachineName}";
                }
                var userData = HockeyConfiguration.SearchDirectoryForUserInformation();
                return string.IsNullOrWhiteSpace(userData[0]) ? $"{Environment.UserDomainName} - {Environment.UserName} - {Environment.MachineName}" : userData[0];
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message.Trim());
                return $"{Environment.UserDomainName} - {Environment.UserName} - {Environment.MachineName}";
            }
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
        /// Get the device Manufacturer and Model that TelemetryClient will report
        /// </summary>
        public static string DeviceDetails =>
            $"{TelemetryClient?.Context.Device.OemName} {TelemetryClient?.Context.Device.Model}";

        /// <summary>
        /// Get the device manufacturer from Management Information
        /// </summary>
        /// <returns>Device manufacturer if it can be found</returns>
        private static string GetDeviceManufacturer()
        {
            const string manufacturer = "Manufacturer";
            return RetrieveValueFromManagementInformation(manufacturer, "Win32_ComputerSystem", "Unknown");
        }

        /// <summary>
        /// Get the device Model from Management Information
        /// </summary>
        /// <returns>Device manufacturer if it can be found</returns>
        private static string GetDeviceModel()
        {
            const string model = "Model";
            return RetrieveValueFromManagementInformation(model, "Win32_ComputerSystem", "Unknown");
        }

        /// <summary>
        /// Retrieve the Windows friendly name instead of just a version
        /// </summary>
        /// <returns></returns>
        private static string GetWindowsFriendlyName()
        {
            const string caption = "Caption";
            const string component = "Win32_OperatingSystem";
            var fallback = Environment.OSVersion.ToString();

            return RetrieveValueFromManagementInformation(caption, component, fallback);
        }

        /// <summary>
        /// Retrieve an entry from ManagementInformation
        /// </summary>
        /// <param name="propertyToRetrieve">The property to retrieve</param>
        /// <param name="componentFromWhichToRetrieve">The component from which the value should be retrieved</param>
        /// <param name="fallbackValue">Value to return if property cannot be found</param>
        /// <returns>Result from the ManagementInformation query</returns>
        private static string RetrieveValueFromManagementInformation(string propertyToRetrieve,
            string componentFromWhichToRetrieve, string fallbackValue)
        {
            return new ManagementObjectSearcher($"SELECT {propertyToRetrieve} FROM {componentFromWhichToRetrieve}")
                       .Get()
                       .OfType<ManagementObject>()
                       .Select(x => x.GetPropertyValue(propertyToRetrieve))
                       .FirstOrDefault()
                       ?.ToString()
                   ?? fallbackValue;
        }
    }
}
