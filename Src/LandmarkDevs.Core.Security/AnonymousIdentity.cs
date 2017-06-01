namespace LandmarkDevs.Core.Security
{
    /// <summary>
    /// Class AnonymousIdentity.
    /// </summary>
    /// <seealso cref="LandmarkDevs.Core.Security.CustomIdentity" />
    public class AnonymousIdentity : CustomIdentity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousIdentity"/> class.
        /// </summary>
        public AnonymousIdentity() : base(string.Empty, string.Empty, new string[] { })
        { }
    }
}