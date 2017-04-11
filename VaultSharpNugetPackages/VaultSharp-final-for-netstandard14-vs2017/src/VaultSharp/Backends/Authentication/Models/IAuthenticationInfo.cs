namespace VaultSharp.Backends.Authentication.Models
{
    /// <summary>
    /// Provides an interface to provide authentication information.
    /// </summary>
    public interface IAuthenticationInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        AuthenticationBackendType AuthenticationBackendType { get; }
    }
}
