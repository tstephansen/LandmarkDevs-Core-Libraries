using Prism.Regions;

namespace LandmarkDevs.Core.Prism
{
    /// <summary>
    ///     A model used for navigating between views.
    /// </summary>
    /// <seealso cref="INavModel" />
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class NavModel : INavModel
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NavModel" /> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="navParam">The nav parameter.</param>
        /// <param name="viewModel">The view model.</param>
        public NavModel(string viewName, NavigationParameters navParam, dynamic viewModel)
        {
            ViewName = viewName;
            NavigationParameters = navParam;
            ViewModel = viewModel;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NavModel" /> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        public NavModel(string viewName)
        {
            ViewName = viewName;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NavModel" /> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="navParam">The nav parameter.</param>
        public NavModel(string viewName, NavigationParameters navParam)
        {
            ViewName = viewName;
            NavigationParameters = navParam;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NavModel" /> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="viewModel">The view model.</param>
        public NavModel(string viewName, dynamic viewModel)
        {
            ViewName = viewName;
            ViewModel = viewModel;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NavModel" /> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public NavModel(dynamic viewModel)
        {
            ViewModel = viewModel;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NavModel" /> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="viewId">The view id.</param>
        public NavModel(dynamic viewModel, string viewId)
        {
            ViewModel = viewModel;
            ViewId = viewId;
        }

        /// <summary>
        ///     Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public dynamic ViewModel { get; set; }

        /// <summary>
        ///     Gets or sets the name of the view.
        /// </summary>
        /// <value>The name of the view.</value>
        public string ViewName { get; set; }

        /// <summary>
        ///     Gets or sets the navigation parameters.
        /// </summary>
        /// <value>The navigation parameters.</value>
        public NavigationParameters NavigationParameters { get; set; }

        /// <summary>
        ///     Gets or sets the view id.
        /// </summary>
        /// <value>The view id.</value>
        public string ViewId { get; set; }
    }
}