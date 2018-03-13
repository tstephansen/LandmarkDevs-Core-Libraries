using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace LandmarkDevs.Core.Infrastructure
{
    /// <summary>
    /// Class ApplicationLogger.
    /// Used for logging operations in the application.
    /// </summary>
    public static class ApplicationLogger
    {
        /// <summary>
        /// Initializes application logging.
        /// </summary>
        /// <returns>ILogger.</returns>
        public static ILogger InitializeLogging()
        {
            try
            {
                var path = CreateAppDataDirectory();
                return InitializeLogging(path, false, null);
            }
            catch
            {
                LogManager.Configuration = new NLog.Config.LoggingConfiguration();
                return _logger = LogManager.GetCurrentClassLogger();
            }
        }

        /// <summary>
        /// Initializes application logging.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>ILogger.</returns>
        public static ILogger InitializeLogging(string path)
        {
            CreateAppDataDirectory(path);
            return InitializeLogging(path, false, null);
        }

        /// <summary>
        /// Initializes application logging.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="remoteLoggingEnabled">if set to <c>true</c> [remote logging enabled].</param>
        /// <param name="remoteLogIpAddress">The remote log ip address.</param>
        /// <returns>ILogger.</returns>
        public static ILogger InitializeLogging(string path, bool remoteLoggingEnabled, string remoteLogIpAddress)
        {
            var filePath = Path.Combine(path, $"{DateTime.Today.Month}-{DateTime.Today.Day}-{DateTime.Today.Year}.log");
            var jsonFilePath = Path.Combine(path, $"{DateTime.Today.Month}-{DateTime.Today.Day}-{DateTime.Today.Year}.json");
            var config = new LoggingConfiguration();
            var jsonLayout = new JsonLayout
            {
                Attributes =
                {
                    new JsonAttribute("date", "${date:format=MM-dd-yyyy}"),
                    new JsonAttribute("time", "${time}"),
                    new JsonAttribute("friendlytime", @"${date:format=HH\:mm\:ss}"),
                    new JsonAttribute("hour", "${date:format=HH}"),
                    new JsonAttribute("minute", "${date:format=mm}"),
                    new JsonAttribute("ticks", "${ticks}"),
                    new JsonAttribute("logger", "${logger}"),
                    new JsonAttribute("level", "${level}"),
                    new JsonAttribute("appDomain", "${appdomain}"),
                    new JsonAttribute("version", "${assembly-version}"),
                    new JsonAttribute("ident", "${identity}"),
                    new JsonAttribute("winIdent", "${windows-identity}"),
                    new JsonAttribute("machine", "${machinename}"),
                    new JsonAttribute("message", "${message}"),
                    new JsonAttribute("exceptionData", new JsonLayout
                    {
                        Attributes =
                        {
                            new JsonAttribute("exception", "${exception}"),
                            new JsonAttribute("stack", "${stacktrace}"),
                            new JsonAttribute("callSite", "${callsite}"),
                            new JsonAttribute("callSite-Line", "${callsite-linenumber}")
                        },
                        RenderEmptyObject = false
                    })
                }
            };
            if (remoteLoggingEnabled)
            {
                var viewerTarget = new NLogViewerTarget
                {
                    Encoding = Encoding.UTF8,
                    IncludeCallSite = true,
                    NewLine = true,
                    OnOverflow = NetworkTargetOverflowAction.Split,
                    MaxConnections = 0,
                    MaxMessageSize = 65565,
                    ConnectionCacheSize = 25,
                    KeepConnection = true,
                    IncludeSourceInfo = true,
                    IncludeMdc = true,
                    IncludeNLogData = true,
                    IncludeNdc = true,
                    Address = $"udp://{remoteLogIpAddress}",
                    Layout = jsonLayout
                };
                config.AddTarget("RemoteViewer", viewerTarget);
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, viewerTarget));
            }
            var fileTarget = new FileTarget
            {
                FileName = filePath,
                Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}"
            };
            config.AddTarget("LogFile", fileTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            var jsonTarget = new FileTarget
            {
                FileName = jsonFilePath,
                Layout = jsonLayout
            };
            config.AddTarget("JsonLogFile", jsonTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, jsonTarget));

            LogManager.Configuration = config;
            _logger = LogManager.GetCurrentClassLogger();
            return _logger;
        }

        private static void CreateAppDataDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private static string CreateAppDataDirectory()
        {
            var assemblyName = Assembly.GetEntryAssembly().GetName().ToString();
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dirPath = Path.Combine(localAppData, assemblyName);
            CreateAppDataDirectory(dirPath);
            return dirPath;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <returns>ILogger.</returns>
        public static ILogger GetLogger()
        {
            return _logger;
        }

        private static ILogger _logger;
    }
}