using System.Threading.Tasks;
using VaultSharp.V1.AuthMethods.Token.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.Token
{
    /// <summary>
    /// Token Auth Method
    /// </summary>
    public interface ITokenAuthMethod
    {
        /// <summary>
        /// Gets the calling client token information. i.e. the token used by the client as part of this call.
        /// </summary>
        /// <returns>
        /// The secret with <see cref="CallingTokenInfo" />.
        /// </returns>
        Task<Secret<CallingTokenInfo>> LookupSelfAsync();

        /// <summary>
        /// Renews a lease associated with the calling token.
        /// This is used to prevent the expiration of a token, and the automatic revocation of it.
        /// Token renewal is possible only if there is a lease associated with it.
        /// </summary>
        /// <param name="increment"><para>[optional]</para>
        /// An optional requested lease increment can be provided. This increment may be ignored.
        /// <returns>
        /// The auth info.
        /// </returns>
        Task<AuthInfo> RenewSelfAsync(string increment = null);
    }
}