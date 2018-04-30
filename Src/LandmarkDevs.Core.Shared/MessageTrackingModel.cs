using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace LandmarkDevs.Core.Shared
{
    /// <summary>
    ///     A data model for change tracking that includes a message when the property value changes.
    /// </summary>
    /// <seealso cref="ChangeTrackingModel" />
    public class MessageTrackingModel : ChangeTrackingModel, IMessageTrackingModel
    {
        #region Constructor
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageTrackingModel" /> class.
        /// </summary>
        public MessageTrackingModel()
        {
            ChangesNotes = new ConcurrentDictionary<string, TrackerMessage>();
        }

        #endregion Constructor

        #region Methods
        /// <summary>
        ///     Tracks the message.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="propertyTitle">The property title.</param>
        /// <param name="propertyName">Name of the property.</param>
        public void TrackMessage(object newValue, object oldValue, string propertyTitle, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName) || oldValue == newValue)
                return;
            var message = string.IsNullOrWhiteSpace(oldValue?.ToString())
                ? $"{propertyTitle} changed from NULL to {newValue}. \n"
                : string.IsNullOrWhiteSpace(newValue?.ToString())
                    ? $"{propertyTitle} changed from {oldValue} to NULL. \n"
                    : $"{propertyTitle} changed from {oldValue} to {newValue}. \n";
            LogMessage(newValue, oldValue, message, propertyName);
        }

        /// <summary>
        ///     Tracks the message.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="message">The message.</param>
        /// <param name="propertyName">Name of the property.</param>
        public void TrackCustomMessage(object newValue, object oldValue, string message, [CallerMemberName] string propertyName = null)
        {
            LogMessage(newValue, oldValue, message, propertyName);
        }

        private void LogMessage(object newValue, object oldValue, string message, string propertyName)
        {
            if (ChangesNotes.ContainsKey(propertyName))
            {
                var originalValue = ChangesNotes[propertyName].OriginalValue;

                if ((string.IsNullOrWhiteSpace(newValue?.ToString())
                     && string.IsNullOrWhiteSpace(originalValue?.ToString()))
                    || (originalValue == newValue))
                {
                    RemoveMessage(propertyName);
                }
                else
                {
                    ChangeMessage(originalValue, message, propertyName);
                }
            }
            else
            {
                LogNewMessage(oldValue, message, propertyName);
            }
        }

        private void LogNewMessage(object oldValue, string message, string propertyName)
        {
            var newChange = new TrackerMessage
            {
                OriginalValue = oldValue,
                Message = message
            };
            if (!ChangesNotes.TryAdd(propertyName, newChange))
                throw new ArgumentException("Unable to add specified property to the change message dictionary.");
            RaiseOnModified(propertyName);
        }

        private void ChangeMessage(object originalValue, string message, string propertyName)
        {
            var newChange = new TrackerMessage
            {
                OriginalValue = originalValue,
                Message = message
            };
            ChangesNotes[propertyName] = newChange;
            RaiseOnModified(propertyName);
        }

        private void RemoveMessage(string propertyName)
        {
            ChangesNotes.TryRemove(propertyName, out var removeValue);
            RaiseOnModified(propertyName);
        }

        /// <summary>
        ///     Resets this instance.
        /// </summary>
        public new virtual void Reset()
        {
            Changes.Clear();
            ChangesNotes.Clear();
            RaiseOnModified("");
        }

        /// <summary>
        ///     Gets a value indicating whether this instance has messages.
        /// </summary>
        /// <value><c>true</c> if this instance has messages; otherwise, <c>false</c>.</value>
        public virtual bool HasMessages => ChangesNotes.Count > 0;

        #endregion Methods

        #region Variables
        /// <summary>
        ///     Gets or sets the changes notes.
        /// </summary>
        /// <value>The changes notes.</value>
        public ConcurrentDictionary<string, TrackerMessage> ChangesNotes { get; set; }

        #endregion Variables
    }
}