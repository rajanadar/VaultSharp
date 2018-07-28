namespace VaultSharp.V1.AuthMethods
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthMethodInfo
    {
        /// <summary>
        /// Gets the type of the authentication method.
        /// </summary>
        /// <value>
        /// The type of the authentication method.
        /// </value>
        AuthMethodType AuthMethodType { get; }
    }
}