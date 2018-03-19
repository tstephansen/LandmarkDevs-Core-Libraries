namespace LandmarkDevs.Infrastructure
{
    /// <summary>
    /// Class EventLogManager.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class EventLogManager
    {
        /// <summary>
        /// Checks to see if the event source exists and creates
        /// it if it doesn't exist.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="logName">Name of the log.</param>
        public static void CheckEventSource(string source, string logName)
        {
            if (!System.Diagnostics.EventLog.SourceExists(source))
                System.Diagnostics.EventLog.CreateEventSource(source, logName);
        }

        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="source">The event source.</param>
        /// <param name="logName">The log name.</param>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        public static void WriteEntry(string source, string logName, string message, System.Diagnostics.EventLogEntryType type)
        {
            CheckEventSource(source, logName);
            System.Diagnostics.EventLog.WriteEntry(source, message, type);
        }
    }
}