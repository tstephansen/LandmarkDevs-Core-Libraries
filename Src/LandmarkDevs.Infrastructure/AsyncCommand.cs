using System;
using System.Reflection;
using System.Threading.Tasks;

namespace LandmarkDevs.Infrastructure
{
    /// <summary>
    ///     This class is based on the DelegateCommand class from the Prism Library. https://github.com/PrismLibrary/Prism/blob/Prismv6.1.0/Source/Prism/Commands/DelegateCommand.cs
    /// </summary>
    /// <seealso cref="AsyncCommandBase" />
    public class AsyncCommand : AsyncCommandBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand" /> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        public AsyncCommand(Action executeMethod) : this(executeMethod, () => true) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand" /> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <exception cref="ArgumentNullException">executeMethod</exception>
        public AsyncCommand(Action executeMethod, Func<bool> canExecuteMethod) : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));
        }

        /// <summary>
        ///     Creates a command that can be executed asynchronously.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <returns>AsyncCommand.</returns>
        public static AsyncCommand AsAsync(Func<Task> executeMethod) => new AsyncCommand(executeMethod);

        /// <summary>
        ///     Creates a command that can be executed asynchronously.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <returns>AsyncCommand.</returns>
        public static AsyncCommand AsAsync(Func<Task> executeMethod, Func<bool> canExecuteMethod) => new AsyncCommand(executeMethod, canExecuteMethod);

        ///<summary>
        /// Executes the command.
        ///</summary>
        public virtual async Task Execute() => await Execute(null);

        private AsyncCommand(Func<Task> executeMethod)
            : this(executeMethod, () => true)
        {
        }

        private AsyncCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod) : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));
        }
    }

    /// <summary>
    ///     Class AsyncCommand. This class is based on the DelegateCommand class from the Prism
    ///     Library. https://github.com/PrismLibrary/Prism/blob/Prismv6.1.0/Source/Prism/Commands/DelegateCommand.cs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Yca.ManagementSystem.Infrastructure.AsyncCommandBase" />
    public class AsyncCommand<T> : AsyncCommandBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand{T}" /> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        public AsyncCommand(Action<T> executeMethod) : this(executeMethod, (o) => true) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncCommand{T}" /> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <exception cref="ArgumentNullException">executeMethod</exception>
        /// <exception cref="InvalidCastException"></exception>
        public AsyncCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod) : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));
            var genericTypeInfo = typeof(T).GetTypeInfo();

            if (genericTypeInfo.IsValueType && ((!genericTypeInfo.IsGenericType) || (!typeof(Nullable<>).GetTypeInfo()
                                                    .IsAssignableFrom(genericTypeInfo.GetGenericTypeDefinition()
                                                        .GetTypeInfo())))) throw new InvalidCastException();
        }

        /// <summary>
        ///     Creates a command that can be executed asynchronously.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <returns>AsyncCommand&lt;T&gt;.</returns>
        public static AsyncCommand<T> AsAsync(Func<T, Task> executeMethod) => new AsyncCommand<T>(executeMethod);

        /// <summary>
        ///     Creates a command that can be executed asynchronously.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <returns>AsyncCommand.</returns>
        public static AsyncCommand<T> AsAsync(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod) => new AsyncCommand<T>(executeMethod, canExecuteMethod);

        /// <summary>
        ///     Executes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Task.</returns>
        public virtual async Task Execute(T parameter) => await base.Execute(parameter);

        private AsyncCommand(Func<T, Task> executeMethod)
            : this(executeMethod, (o) => true)
        {
        }

        private AsyncCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod) : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));
        }
    }
}