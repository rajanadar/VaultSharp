using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods
{
    /// <summary>
    /// Auth Method login provider.
    /// </summary>
    internal interface IAuthMethodLoginProvider
    {
        /// <summary>
        /// Login Api call.
        /// </summary>
        /// <returns>Login reponse with token.</returns>
        // Task<Secret<Dictionary<string, object>>> LoginAsync();

        /// <summary>
        /// The login method for the auth method.
        /// </summary>
        /// <returns>The Vault Token.</returns>
        Task<string> GetVaultTokenAsync();
    }
}
