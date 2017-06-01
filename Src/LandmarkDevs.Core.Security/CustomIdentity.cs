using System.Security.Principal;

namespace LandmarkDevs.Core.Security
{
    /// <summary>
    /// Class CustomIdentity.
    /// </summary>
    /// <seealso cref="System.Security.Principal.IIdentity" />
    public class CustomIdentity : IIdentity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomIdentity"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <param name="roles">The roles.</param>
        public CustomIdentity(string name, string email, string[] roles)
        {
            Name = name;
            Email = email;
            Roles = roles;
        }

        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public string[] Roles { get; }

        #region IIdentity Members
        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        /// <value>The type of the authentication.</value>
        public string AuthenticationType => "Custom Authentication";

        /// <summary>
        /// Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        public bool IsAuthenticated => !string.IsNullOrEmpty(Name);

        #endregion
    }
}