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
    }
}