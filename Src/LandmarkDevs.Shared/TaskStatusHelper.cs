using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LandmarkDevs.Shared
{
    /// <summary>
    ///     A helper used to report the status of a task that is in progress.
    /// </summary>
    /// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged" />
    /// <inheritdoc />
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TaskStatusHelper : INotifyPropertyChanged
    {
        private double progressPercentage;
        /// <summary>
        ///     Gets or sets the progress percentage.
        /// </summary>
        /// <value>The progress percentage.</value>
        public double ProgressPercentage
        {
            get => progressPercentage;
            set
            {
                progressPercentage = value;
                OnPropertyChanged();
            }
        }

        private string progressText;
        /// <summary>
        ///     Gets or sets the progress text.
        /// </summary>
        /// <value>The progress text.</value>
        public string ProgressText
        {
            get => progressText;
            set
            {
                if (progressText == value) return;
                progressText = value;
                OnPropertyChanged();
            }
        }

        private string text;
        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Called when the property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}