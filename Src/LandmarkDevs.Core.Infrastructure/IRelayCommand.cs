namespace LandmarkDevs.Core.Infrastructure
{
    /// <inheritdoc/>
    public interface IRelayCommand : System.Windows.Input.ICommand
    {
        /// <summary>
        ///     Called when the can execute value has changed.
        /// </summary>
        void OnCanExecuteChanged();

        /// <summary>
        ///     Raises <see cref="System.Windows.Input.ICommand.CanExecuteChanged"/> so every command
        ///     invoker can requery to check if the command can execute.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}