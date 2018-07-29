using VaultSharp.V1.Commons;

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

        /// <summary>
        /// Gets the returned Login Auth info from Vault.
        /// It'll have the vault login token, accessors and any login metadata.
        /// </summary>
        AuthInfo ReturnedLoginAuthInfo { get; }
    }
}