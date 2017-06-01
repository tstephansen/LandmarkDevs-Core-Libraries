using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LandmarkDevs.Core.Shared
{
    /// <summary>
    /// Class TaskStatusHelper.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TaskStatusHelper : INotifyPropertyChanged
    {
        private double _progressPercentage;
        /// <summary>
        /// Gets or sets the progress percentage.
        /// </summary>
        /// <value>The progress percentage.</value>
        public double ProgressPercentage
        {
            get { return _progressPercentage; }
            set
            {
                _progressPercentage = value;
                OnPropertyChanged();
            }
        }

        private string _progressText;
        /// <summary>
        /// Gets or sets the progress text.
        /// </summary>
        /// <value>The progress text.</value>
        public string ProgressText
        {
            get { return _progressText; }
            set
            {
                if (_progressText != value)
                {
                    _progressText = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _text;
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when the property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}