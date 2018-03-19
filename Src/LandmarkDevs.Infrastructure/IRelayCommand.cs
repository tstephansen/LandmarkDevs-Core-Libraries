namespace LandmarkDevs.Infrastructure
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
        /// <summary>
        /// Raises <see cref="CanExecuteChanged"/> so every command invoker
        /// can requery to check if the command can execute.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}