using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LandmarkDevs.Core.Shared
{
    /// <summary>
    /// A data model for change tracking.
    /// </summary>
    /// <seealso cref="LandmarkDevs.Core.Shared.IChangeTrackingModel" />
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public abstract class ChangeTrackingModel : IChangeTrackingModel
    {
        #region Methods
        /// <summary> Resets this object. </summary>
        public virtual void Reset()
        {
            Changes.Clear();
            RaiseOnModified("");
        }

        /// <summary> Tracks the changed value. </summary>
        /// <exception cref="ArgumentException">
        ///  Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <param name="value"> The value. </param>
        /// <param name="propertyName">
        ///  (Optional)
        ///  Name of the property.
        ///  </param>
        public void TrackChange(object value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return;
            if (Changes.ContainsKey(propertyName))
            {
                LogChange(value, propertyName);
            }
            else
            {
                if (!Changes.TryAdd(propertyName, GetPropertyValue(propertyName)))
                    throw new ArgumentException("Unable to add specified property to the changed data dictionary.");
                RaiseOnModified(propertyName);
            }
        }

        private void LogChange(object value, string propertyName)
        {
            object valueObject;
            if (Changes[propertyName] == null)
            {
                if (!string.IsNullOrWhiteSpace(value?.ToString()))
                    return;
                Changes.TryRemove(propertyName, out valueObject);
                RaiseOnModified(propertyName);
            }
            else
            {
                if (!Changes[propertyName].Equals(value))
                    return;
                Changes.TryRemove(propertyName, out valueObject);
                RaiseOnModified(propertyName);
            }
        }

        private object GetPropertyValue(string property)
        {
            return string.IsNullOrWhiteSpace(property) ? null : GetType().GetProperty(property)?.GetValue(this, null);
        }

        /// <summary> True if this object has changes. </summary>
        public virtual bool HasChanges => Changes.Count > 0;

        /// <summary> Gets changed value. </summary>
        /// <param name="propertyName"> Name of the property. </param>
        /// <returns> The changed value. </returns>
        public object GetChangedValue(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !Changes.ContainsKey(propertyName))
                return null;
            return Changes[propertyName];
        }

        #endregion Methods

        #region Events        
        /// <summary>
        /// Occurs when the property is modified.
        /// </summary>
        public event PropertyChangedEventHandler OnModified;

        /// <summary>
        /// Raises the on modified event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void RaiseOnModified(string propertyName)
        {
            OnModified?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            OnPropertyChanged();
        }

        #endregion Events

        #region Properties
        /// <summary>Gets or sets the changes.</summary>
        /// <value>The changes.</value>
        public ConcurrentDictionary<string, object> Changes
        {
            get { return _changes; }

            set
            {
                if (Equals(_changes, value))
                    return;
                _changes = value;
                OnPropertyChanged();
            }
        }

        private ConcurrentDictionary<string, object> _changes = new ConcurrentDictionary<string, object>();
        #endregion Properties

        #region INPC        
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property value changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INPC
    }
}