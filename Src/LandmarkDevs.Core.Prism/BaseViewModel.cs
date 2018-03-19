using LandmarkDevs.Core.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Prism.Events;
using Prism.Unity;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using LandmarkDevs.Core.Telemetry;
#pragma warning disable S3881

namespace LandmarkDevs.Core.Prism
{
    /// <summary>
    /// The base for all of the view models in the application.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        public BaseViewModel()
        {
            Logger = ServiceLocator.Current != null 
                ? ServiceLocator.Current.TryResolve<ILogger>() 
                ?? ApplicationLogger.InitializeLogging() 
                : ApplicationLogger.InitializeLogging();
        }

        #endregion

        #region Fields
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        private string _title;

        /// <summary>
        /// Gets or sets the view identifier.
        /// </summary>
        /// <value>The view identifier.</value>
        public string ViewId
        {
            get => _viewId;
            set => Set(ref _viewId, value);
        }
        private string _viewId;

        /// <summary>
        /// Gets or sets the visual state.
        /// </summary>
        /// <value>The visual state.</value>
        public string VisualState
        {
            get => _visualState;
            set => Set(ref _visualState, value);
        }

        private string _visualState;

        /// <summary>
        /// Gets the event aggregator.
        /// </summary>
        /// <value>The event aggregator.</value>
        [ExcludeFromCodeCoverage]
        public virtual IEventAggregator EventAggregator { get; set; } = ServiceLocator.Current.GetInstance<IEventAggregator>();

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        [ExcludeFromCodeCoverage]
        public virtual ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the telemetry tracker.
        /// </summary>
        /// <value>The telemetry tracker.</value>
        [ExcludeFromCodeCoverage]
        public virtual ITelemetryTracker TelemetryTracker { get; set; } = ServiceLocator.Current.GetInstance<ITelemetryTracker>();
        #endregion

        #region IDisposable Support
        private bool _disposedValue;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
                return;
            if (disposing)
            {
                // Not implemented here.
            }
            _disposedValue = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region INPC
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when a property is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [ExcludeFromCodeCoverage]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the property to the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">The property who's value will change.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }
            storage = value;
            OnPropertyChanged(propertyName);
        }
        #endregion
    }
}