using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Unity;
#pragma warning disable S3881

namespace LandmarkDevs.Core.Prism
{
    /// <summary>
    /// Used as the base view model for Prism Applications.
    /// </summary>
    /// <seealso cref="LandmarkDevs.Core.Prism.BaseViewModel" />
    public class UnityBaseViewModel : BaseViewModel
    {
        public UnityBaseViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                var container = new UnityContainer();
                ServiceLocator.SetLocatorProvider(() => new UnityServiceLocatorAdapter(container));
                return;
            }
        }
    }
}