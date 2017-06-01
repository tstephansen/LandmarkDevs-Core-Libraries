using System.Linq;
using System.Security.Principal;

namespace LandmarkDevs.Core.Security
{
    /// <summary>
    /// Class CustomPrincipal.
    /// </summary>
    /// <seealso cref="System.Security.Principal.IPrincipal" />
    public class CustomPrincipal : IPrincipal
    {
        private CustomIdentity _identity;
        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <value>The identity.</value>
        public CustomIdentity Identity
        {
            get => _identity ?? new AnonymousIdentity();
            set => _identity = value;
        }

        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <value>The identity.</value>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        IIdentity IPrincipal.Identity
        {
            [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
            get
            {
                return Identity;
            }
        }

        #region IPrincipal Members
        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <param name="role">The name of the role for which to check membership.</param>
        /// <returns>true if the current principal is a member of the specified role; otherwise, false.</returns>
        public bool IsInRole(string role) => _identity.Roles.Contains(role);

        #endregion
    }
}