using System;

namespace LandmarkDevs.Infrastructure
{
    /// <summary>
    /// Class ErrorModel.
    /// Used to pass errors to the error log in a separate assembly.
    /// </summary>
    /// <seealso cref="IErrorModel" />
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ErrorModel : IErrorModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModel"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="managerId">The manager identifier.</param>
        /// <param name="location">The location.</param>
        public ErrorModel(Exception ex, Guid managerId, string location)
        {
            Exception = ex;
            ManagerId = managerId;
            Location = location;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModel"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="managerId">The manager identifier.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="location">The location.</param>
        public ErrorModel(Exception ex, Guid managerId, string fullName, string location)
        {
            Exception = ex;
            ManagerId = managerId;
            Location = location;
            FullName = fullName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModel"/> class.
        /// </summary>
        /// <param name="applicationName">The application name.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="managerId">The manager identifier.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="location">The location.</param>
        public ErrorModel(string applicationName, Exception ex, Guid managerId, string fullName, string location)
        {
            ApplicationName = applicationName;
            Exception = ex;
            ManagerId = managerId;
            Location = location;
            FullName = fullName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModel"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="location">The location.</param>
        public ErrorModel(Exception ex, string fullName, string location)
        {
            Exception = ex;
            Location = location;
            FullName = fullName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorModel"/> class.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="location">The location.</param>
        public ErrorModel(string applicationName, Exception ex, string fullName, string location)
        {
            ApplicationName = applicationName;
            Exception = ex;
            Location = location;
            FullName = fullName;
        }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the manager identifier.
        /// </summary>
        /// <value>The manager identifier.</value>
        public Guid ManagerId { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string ApplicationName { get; set; }
    }
}