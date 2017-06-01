using LandmarkDevs.Core.Infrastructure;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LandmarkDevs.Core.Telemetry
{
    /// <summary>
    /// Class TelemetryErrorTracker.
    /// </summary>
    /// <seealso cref="LandmarkDevs.Core.Infrastructure.IErrorTracker" />
    public class TelemetryErrorTracker : IErrorTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorTracker"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="tracker">The tracker.</param>
        /// <exception cref="System.ArgumentNullException">
        /// logger
        /// or
        /// tracker
        /// </exception>
        public TelemetryErrorTracker(ILogger logger, ITelemetryTracker tracker)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
        }

        private readonly ILogger _logger;
        private readonly ITelemetryTracker _tracker;

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        public virtual int LogError(IErrorModel model)
        {
            try
            {
                _tracker.TrackError(model.Exception, model.Location, model.FullName);
                _logger.Log(LogLevel.Error, model.Exception.ToString());
                var path = Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(), model.ApplicationName);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var logFile = path + "\\ErrorLog.txt";
                using (var stream = new StreamWriter(logFile))
                {
                    var logText = $"Date & Time: {DateTime.Now}\nUser: {model.FullName}\nLocation: {model.Location}\nError: {model.Exception.Message.Trim()}\n----------------------------------------";
                    stream.Write(logText);
                    stream.Flush();
                }
                return 0;
            }
            // lol I'm not going to try to log an error about logging an error...
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message.Trim());
                return -1;
            }
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public virtual async Task<int> LogErrorAsync(IErrorModel model)
        {
            try
            {
                _tracker.TrackError(model.Exception, model.Location, model.FullName);
                _logger.Log(LogLevel.Error, model.Exception.ToString());
                var path = Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(), model.ApplicationName);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var logFile = path + "\\ErrorLog.txt";
                using (var stream = new StreamWriter(logFile))
                {
                    var logText = $"Date & Time: {DateTime.Now}\nUser: {model.FullName}\nLocation: {model.Location}\nError: {model.Exception.Message.Trim()}\n----------------------------------------";
                    await stream.WriteAsync(logText);
                    stream.Flush();
                }
                return 0;
            }
            // lol I'm not going to try to log an error about logging an error...
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message.Trim());
                return -1;
            }
        }
    }
}