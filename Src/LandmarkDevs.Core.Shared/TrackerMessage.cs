namespace LandmarkDevs.Core.Shared
{
    /// <summary>
    /// Struct TrackerMessage. Used for tracking messages in the <seealso cref="MessageTrackingModel"/>.
    /// </summary>
    public struct TrackerMessage
    {
        /// <summary>
        /// The original value
        /// </summary>
        public object OriginalValue { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
    }
}