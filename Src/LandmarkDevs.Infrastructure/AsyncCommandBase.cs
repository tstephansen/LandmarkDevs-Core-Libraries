using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LandmarkDevs.Infrastructure
{
    /// <summary>
    /// Class AsyncCommand. This class is based on the DelegateCommandBase class from the Prism Library.
    /// https://github.com/PrismLibrary/Prism/blob/Prismv6.1.0/Source/Prism/Commands/DelegateCommandBase.cs
    /// </summary>
    /// <seealso cref="ICommand" />
    public abstract class AsyncCommandBase : ICommand
    {
        protected readonly Func<object, Task> ExecuteMethod;
        protected readonly Func<object, bool> CanExecuteMethod;

        protected AsyncCommandBase(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));
            ExecuteMethod = (arg) => { executeMethod(arg); return Task.Delay(0); };
            CanExecuteMethod = canExecuteMethod;
        }
        
        protected AsyncCommandBase(Func<object, Task> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));
            ExecuteMethod = executeMethod;
            CanExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Called when the can execute value has changed.
        /// </summary>
        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        async void ICommand.Execute(object parameter)
        {
            await Execute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        /// <summary>
        /// Executes the command with the provided parameter by invoking the <see cref="Action{Object}"/> supplied during construction.
        /// </summary>
        /// <param name="parameter"></param>
        protected async Task Execute(object parameter)
        {
            await ExecuteMethod(parameter);
        }

        /// <summary>
        /// Determines if the command can execute with the provided parameter by invoking the <see cref="Func{Object,Bool}"/> supplied during construction.
        /// </summary>
        /// <param name="parameter">The parameter to use when determining if this command can execute.</param>
        /// <returns>Returns <see langword="true"/> if the command can execute.  <see langword="False"/> otherwise.</returns>
        protected bool CanExecute(object parameter)
        {
            return CanExecuteMethod(parameter);
        }
    }
}