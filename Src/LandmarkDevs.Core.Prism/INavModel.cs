using Prism.Regions;

namespace LandmarkDevs.Core.Prism
{
    /// <summary>
    /// Interface INavModel
    /// </summary>
    public interface INavModel
    {
        /// <summary>
        /// Gets or sets the view id.
        /// </summary>
        /// <value>The view id.</value>
        string ViewId { get; set; }

        /// <summary>
        /// Gets or sets the name of the view.
        /// </summary>
        /// <value>The name of the view.</value>
        string ViewName { get; set; }

        /// <summary>
        /// Gets or sets the navigation parameters.
        /// </summary>
        /// <value>The navigation parameters.</value>
        NavigationParameters NavigationParameters { get; set; }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        dynamic ViewModel { get; set; }
    }
}