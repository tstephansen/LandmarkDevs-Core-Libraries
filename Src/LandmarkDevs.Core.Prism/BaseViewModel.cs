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
using System.Windows;

namespace LandmarkDevs.Core.Prism
{
    /// <summary>
    /// Class BaseViewModel.
    /// The base for all of the view models in the application. This class contains the navigation,
    /// logging, and unity methods.
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
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                var container = new UnityContainer();
                ServiceLocator.SetLocatorProvider(() => new UnityServiceLocatorAdapter(container));
                return;
            }
            Logger = ServiceLocator.Current.TryResolve<ILogger>() ?? ApplicationLogger.InitializeLogging();
        }

        #endregion Constructor

        #region Fields
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return _title; }

            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _title;

        /// <summary>
        /// Gets or sets the view identifier.
        /// </summary>
        /// <value>The view identifier.</value>
        public string ViewId
        {
            get { return _viewId; }

            set
            {
                if (_viewId != value)
                {
                    _viewId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _viewId;

        /// <summary>
        /// Gets or sets the visual state.
        /// </summary>
        /// <value>The visual state.</value>
        public string VisualState
        {
            get { return _visualState; }

            set
            {
                if (_visualState != value)
                {
                    _visualState = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _visualState;

        /// <summary>
        /// Gets the event aggregator.
        /// </summary>
        /// <value>The event aggregator.</value>
        [ExcludeFromCodeCoverage]
        public virtual IEventAggregator EventAggregator { get; } = ServiceLocator.Current.GetInstance<IEventAggregator>();

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        [ExcludeFromCodeCoverage]
        public virtual ILogger Logger { get; } = ServiceLocator.Current.GetInstance<ILogger>();

        #endregion Fields

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

        #endregion IDisposable Support

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
        #endregion INPC
    }
}