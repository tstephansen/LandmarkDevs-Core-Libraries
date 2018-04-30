using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace LandmarkDevs.Infrastructure
{
    /// <inheritdoc />
    public class RelayCommand : IRelayCommand
    {
        /// <summary>
        ///     Creates a new instance of <see cref="RelayCommand" />.
        /// </summary>
        public RelayCommand()
        {
            synchronizationContext = SynchronizationContext.Current;
        }

        private readonly Action execute;
        private readonly Func<bool> canExecute;
        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RelayCommand(Action execute) : this(execute, null)
        {
            synchronizationContext = SynchronizationContext.Current;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="System.ArgumentNullException">execute</exception>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
            synchronizationContext = SynchronizationContext.Current;
        }

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this
        ///     object can be set to null.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter) => canExecute == null || canExecute();

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this
        ///     object can be set to null.
        /// </param>
        public void Execute(object parameter) => execute();

        /// <summary>
        ///     Called when the can execute value has changed.
        /// </summary>
        public virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                if (synchronizationContext != null && synchronizationContext != SynchronizationContext.Current)
                    synchronizationContext.Post((o) => handler.Invoke(this, EventArgs.Empty), null);
                else
                    handler.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Raises <see cref="CanExecuteChanged" /> so every command invoker can requery to check
        ///     if the command can execute.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        ///     Observes a property that implements INotifyPropertyChanged, and automatically calls
        ///     DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <typeparam name="T">
        ///     The object type containing the property specified in the expression.
        /// </typeparam>
        /// <param name="propertyExpression">
        ///     The property expression. Example: ObservesProperty(() =&gt; PropertyName).
        /// </param>
        protected internal void ObservesPropertyInternal<T>(Expression<Func<T>> propertyExpression)
        {
            if (observedPropertiesExpressions.Contains(propertyExpression.ToString()))
            {
                throw new ArgumentException($"{propertyExpression.ToString()} is already being observed.",
                    nameof(propertyExpression));
            }
            else
            {
                observedPropertiesExpressions.Add(propertyExpression.ToString());
                PropertyObserver.Observes(propertyExpression, RaiseCanExecuteChanged);
            }
        }

        private SynchronizationContext synchronizationContext;
        private readonly HashSet<string> observedPropertiesExpressions = new HashSet<string>();
    }

    /// <inheritdoc />
    public class RelayCommand<T> : IRelayCommand
    {
        private readonly Action<T> execute;
        private Func<T, bool> canExecute;
        private SynchronizationContext synchronizationContext;
        private readonly HashSet<string> observedPropertiesExpressions = new HashSet<string>();
        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand{T}" /> class.
        /// </summary>
        public RelayCommand()
        {
            synchronizationContext = SynchronizationContext.Current;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand{T}" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RelayCommand(Action<T> execute) : this(execute, null)
        {
            synchronizationContext = SynchronizationContext.Current;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand{T}" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="System.ArgumentNullException">execute</exception>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            synchronizationContext = SynchronizationContext.Current;
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this
        ///     object can be set to null.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter) => canExecute == null || canExecute((T)parameter);

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this
        ///     object can be set to null.
        /// </param>
        public void Execute(object parameter) => execute((T)parameter);

        /// <summary>
        ///     Raises <see cref="ICommand.CanExecuteChanged" /> so every command invoker can requery
        ///     <see cref="ICommand.CanExecute" />.
        /// </summary>
        public virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler == null) return;
            if (synchronizationContext != null && synchronizationContext != SynchronizationContext.Current)
                synchronizationContext.Post((o) => handler.Invoke(this, EventArgs.Empty), null);
            else
                handler.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Raises <see cref="CanExecuteChanged" /> so every command invoker can requery to check
        ///     if the command can execute.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        ///     Observes a property that implements INotifyPropertyChanged, and automatically calls
        ///     DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <typeparam name="Y">
        ///     The object type containing the property specified in the expression.
        /// </typeparam>
        /// <param name="propertyExpression">
        ///     The property expression. Example: ObservesProperty(() =&gt; PropertyName).
        /// </param>
        protected internal void ObservesPropertyInternal<Y>(Expression<Func<Y>> propertyExpression)
        {
            if (observedPropertiesExpressions.Contains(propertyExpression.ToString()))
            {
                throw new ArgumentException($"{propertyExpression} is already being observed.",
                    nameof(propertyExpression));
            }
            observedPropertiesExpressions.Add(propertyExpression.ToString());
            PropertyObserver.Observes(propertyExpression, RaiseCanExecuteChanged);
        }
    }
}