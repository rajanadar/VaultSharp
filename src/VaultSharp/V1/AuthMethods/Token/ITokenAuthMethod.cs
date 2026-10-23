using System.Threading;
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
        /// Creates a new token. 
        /// Certain options are only available when called by a root token. 
        /// If you are creating an orphaned token, a root token is not required to create an orphan token 
        /// (otherwise set with the no_parent option). 
        /// If used with a role name, the token will be created against the specified role name; 
        /// this may override options set during this call.
        /// </summary>
        /// <param name="createTokenRequest">The token creation request</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Auth info</returns>
        Task<Secret<object>> CreateTokenAsync(CreateTokenRequest createTokenRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets token information about the specified token.
        /// </summary>
        /// <param name="clientToken">The vault token to lookup</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The secret with <see cref="TokenInfo" />.
        /// </returns>
        Task<Secret<ClientTokenInfo>> LookupAsync(string clientToken, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the calling client token information. i.e. the token used by the client as part of this call.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The secret with <see cref="CallingTokenInfo" />.
        /// </returns>
        Task<Secret<CallingTokenInfo>> LookupSelfAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Renews a lease associated with the calling token.
        /// This is used to prevent the expiration of a token, and the automatic revocation of it.
        /// Token renewal is possible only if there is a lease associated with it.
        /// </summary>
        /// <param name="increment"><para>[optional]</para>
        /// An optional requested lease increment can be provided. This increment may be ignored.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The auth info.
        /// </returns>
        Task<AuthInfo> RenewSelfAsync(string increment = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes the calling client token and all child tokens.
        /// When the token is revoked, all secrets generated with it are also revoked.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RevokeSelfAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a token role.
        /// </summary>
        /// <param name="roleName">Name of the Token Role.</param>
        /// <param name="createTokenRoleRequest">
        /// Request object containing configurations for the token role.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>No return value.</returns>
        Task CreateTokenRoleAsync(string roleName, CreateTokenRoleRequest createTokenRoleRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns metadata of a token role.
        /// </summary>
        /// <param name="roleName">Token Role to be fetched.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Metadata of requested token role.</returns>
        Task <Secret<TokenRoleInfo>> ReadTokenRoleAsync(string roleName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the available token roles.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Keys of available token roles.</returns>
        Task <Secret<ListTokenRoles>> ListTokenRolesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete token role.
        /// </summary>
        /// <param name="roleName">Token role to be deleted.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>No return value.</returns>
        Task DeleteTokenRoleAsync(string roleName, CancellationToken cancellationToken = default);
    }
}