/*
 * This class was copied directly from the Prism Library so the entire library would not be required
 * as a dependency. The original class can be found here.
 * https://github.com/PrismLibrary/Prism/blob/master/Source/Prism/Commands
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace LandmarkDevs.Core.Infrastructure
{
    /// <summary>
    /// Provide a way to observe property changes of INotifyPropertyChanged objects and invokes a 
    /// custom action when the PropertyChanged event is fired.
    /// </summary>
    internal class PropertyObserver
    {
        private readonly Action _action;

        private PropertyObserver(Expression propertyExpression, Action action)
        {
            _action = action;
            SubscribeListeners(propertyExpression);
        }

        private void SubscribeListeners(Expression propertyExpression)
        {
            var propNameStack = new Stack<string>();
            while (propertyExpression is MemberExpression temp) // Gets the root of the property chain.
            {
                propertyExpression = temp.Expression;
                propNameStack.Push(temp.Member.Name); // Records the name of each property.
            }

            if (!(propertyExpression is ConstantExpression constantExpression))
                throw new NotSupportedException("Operation not supported for the given expression type. " +
                                                "Only MemberExpression and ConstantExpression are currently supported.");

            var propObserverNodeRoot = new PropertyObserverNode(propNameStack.Pop(), _action);
            PropertyObserverNode previousNode = propObserverNodeRoot;
            foreach (var propName in propNameStack) // Create a node chain that corresponds to the property chain.
            {
                var currentNode = new PropertyObserverNode(propName, _action);
                previousNode.Next = currentNode;
                previousNode = currentNode;
            }

            object propOwnerObject = constantExpression.Value;

            if (!(propOwnerObject is INotifyPropertyChanged inpcObject))
                throw new InvalidOperationException("Trying to subscribe PropertyChanged listener in object that " +
                                                    $"owns '{propObserverNodeRoot.PropertyName}' property, but the object does not implements INotifyPropertyChanged.");

            propObserverNodeRoot.SubscribeListenerFor(inpcObject);
        }

        /// <summary>
        /// Observes a property that implements INotifyPropertyChanged, and automatically calls a custom action on 
        /// property changed notifications. The given expression must be in this form: "() => Prop.NestedProp.PropToObserve".
        /// </summary>
        /// <param name="propertyExpression">Expression representing property to be observed. Ex.: "() => Prop.NestedProp.PropToObserve".</param>
        /// <param name="action">Action to be invoked when PropertyChanged event occours.</param>
        internal static PropertyObserver Observes<T>(Expression<Func<T>> propertyExpression, Action action)
        {
            return new PropertyObserver(propertyExpression.Body, action);
        }
    }

    /// <summary>
    /// Represents each node of nested properties expression and takes care of 
    /// subscribing/unsubscribing INotifyPropertyChanged.PropertyChanged listeners on it.
    /// </summary>
    internal class PropertyObserverNode
    {
        private readonly Action _action;
        private INotifyPropertyChanged _inpcObject;

        public string PropertyName { get; }
        public PropertyObserverNode Next { get; set; }

        public PropertyObserverNode(string propertyName, Action action)
        {
            PropertyName = propertyName;
            _action = () =>
            {
                action?.Invoke();
                if (Next == null) return;
                Next.UnsubscribeListener();
                GenerateNextNode();
            };
        }

        public void SubscribeListenerFor(INotifyPropertyChanged inpcObject)
        {
            _inpcObject = inpcObject;
            _inpcObject.PropertyChanged += OnPropertyChanged;

            if (Next != null) GenerateNextNode();
        }

        private void GenerateNextNode()
        {
            var propertyInfo = _inpcObject.GetType().GetRuntimeProperty(PropertyName); // TODO: To cache, if the step consume significant performance. Note: The type of _inpcObject may become its base type or derived type.
            var nextProperty = propertyInfo.GetValue(_inpcObject);
            if (nextProperty == null) return;
            if (!(nextProperty is INotifyPropertyChanged nextInpcObject))
                throw new InvalidOperationException("Trying to subscribe PropertyChanged listener in object that " +
                                                    $"owns '{Next.PropertyName}' property, but the object does not implements INotifyPropertyChanged.");

            Next.SubscribeListenerFor(nextInpcObject);
        }

        private void UnsubscribeListener()
        {
            if (_inpcObject != null)
                _inpcObject.PropertyChanged -= OnPropertyChanged;

            Next?.UnsubscribeListener();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Invoke action when e.PropertyName == null in order to satisfy:
            //  - DelegateCommandFixture.GenericDelegateCommandObservingPropertyShouldRaiseOnEmptyPropertyName
            //  - DelegateCommandFixture.NonGenericDelegateCommandObservingPropertyShouldRaiseOnEmptyPropertyName
            if (e?.PropertyName == PropertyName || e?.PropertyName == null)
            {
                _action?.Invoke();
            }
        }
    }
}