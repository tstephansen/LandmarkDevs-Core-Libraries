using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LandmarkDevs.Infrastructure
{
    /// <summary>
    ///     Tracks errors in the application.
    /// </summary>
    /// <seealso cref="IErrorTracker" />
    public class ErrorTracker : IErrorTracker
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ErrorTracker" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException">logger or tracker</exception>
        public ErrorTracker(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private readonly ILogger logger;

        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        public virtual int LogError(IErrorModel model)
        {
            try
            {
                logger.Log(LogLevel.Error, model.Exception.ToString());
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
        ///     Logs the error.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public virtual async Task<int> LogErrorAsync(IErrorModel model)
        {
            try
            {
                logger.Log(LogLevel.Error, model.Exception.ToString());
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