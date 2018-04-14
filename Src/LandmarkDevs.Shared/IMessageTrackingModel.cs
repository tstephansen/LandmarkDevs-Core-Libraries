using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LandmarkDevs.Shared
{
    /// <summary>
    ///     A data model for change tracking that includes a message when the property value changes.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public interface IMessageTrackingModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Sets the property to the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">The property who's value will change.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        void Set<T>(ref T storage, T value, [CallerMemberName]
            string propertyName = null);

        /// <summary>
        ///     Tracks the message.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="propertyTitle">The property title.</param>
        /// <param name="propertyName">Name of the property.</param>
        void TrackMessage(object newValue, object oldValue, string propertyTitle, [CallerMemberName] string propertyName = null);

        /// <summary>
        ///     Tracks the message.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="message">The message.</param>
        /// <param name="propertyName">Name of the property.</param>
        void TrackCustomMessage(object newValue, object oldValue, string message, [CallerMemberName] string propertyName = null);

        /// <summary>
        ///     Gets a value indicating whether this instance has messages.
        /// </summary>
        /// <value><c>true</c> if this instance has messages; otherwise, <c>false</c>.</value>
        bool HasMessages { get; }

        /// <summary>
        ///     Gets or sets the changes notes.
        /// </summary>
        /// <value>The changes notes.</value>
        ConcurrentDictionary<string, TrackerMessage> ChangesNotes { get; set; }

        /// <summary>
        ///     True if this object has changes.
        /// </summary>
        bool HasChanges { get; }

        /// <summary>
        ///     Gets or sets the changes.
        /// </summary>
        /// <value>The changes.</value>
        ConcurrentDictionary<string, object> Changes { get; set; }

        /// <summary>
        ///     Resets this object.
        /// </summary>
        void Reset();

        /// <summary>
        ///     Tracks the changed value.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">(Optional) Name of the property.</param>
        void TrackChange(object value, [CallerMemberName] string propertyName = null);

        /// <summary>
        ///     Gets changed value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The changed value.</returns>
        object GetChangedValue(string propertyName);

        /// <summary>
        ///     Occurs when the property is modified.
        /// </summary>
        event PropertyChangedEventHandler OnModified;

        /// <summary>
        ///     Raises the on modified event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        void RaiseOnModified(string propertyName);
    }
}