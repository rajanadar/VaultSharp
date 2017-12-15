namespace VaultSharp.Backends.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthInfo
    {
        /// <summary>
        /// Gets the type of the authentication backend.
        /// </summary>
        /// <value>
        /// The type of the authentication backend.
        /// </value>
        AuthBackendType BackendType { get; }
    }
}