namespace LandmarkDevs.Core.Infrastructure
{
    /// <summary>
    /// Interface IRelayCommand
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public interface IRelayCommand : System.Windows.Input.ICommand
    {
        /// <summary>
        /// Called when the can execute value has changed.
        /// </summary>
        void OnCanExecuteChanged();
    }
}