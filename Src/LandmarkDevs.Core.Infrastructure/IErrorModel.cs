using System;

namespace LandmarkDevs.Core.Infrastructure
{
    /// <summary>
    /// Interface IErrorModel
    /// </summary>
    public interface IErrorModel
    {
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the manager identifier.
        /// </summary>
        /// <value>The manager identifier.</value>
        Guid ManagerId { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        string Location { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        string FullName { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        string ApplicationName { get; set; }
    }
}