namespace LandmarkDevs.Core.Infrastructure
{
    /// <summary>
    /// Class Observable. Used to pass property change notifications
    /// from the view to the viewmodel.
    /// Taken from the Windows App Template
    /// </summary>
    /// <example>
    /// private string _propertyName;
    /// public string PropertyName
    /// {
    ///     get => _propertyName;
    ///     set => Set(ref _propertyName, value);
    /// }
    /// </example>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class Observable : System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the property to the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">The property who's value will change.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void Set<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }
            storage = value;
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Called when the property value changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
    }
}