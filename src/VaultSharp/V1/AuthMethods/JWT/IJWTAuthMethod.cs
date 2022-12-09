using System.Threading.Tasks;
using VaultSharp.V1.AuthMethods.JWT.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.JWT
{
    /// <summary>
    /// Non Login methods
    /// </summary>
    public interface IJWTAuthMethod
    {
        /// <summary>
        /// Obtain an authorization URL from Vault to start an OIDC login flow.
        /// </summary>
        /// <param name="redirectUri">
        /// <para>[required]</para>
        /// Path to the callback to complete the login. 
        /// This will be of the form, "https://.../oidc/callback" where the leading portion is dependent on
        /// your Vault server location, port, and the mount of the JWT plugin. 
        /// This must be configured with Vault and the provider.
        /// </param>
        /// <param name="roleName">
        /// <para>[optional]</para>
        /// Name of the role against which the login is being attempted. 
        /// Defaults to configured default_role if not provided.
        /// </param>
        /// <param name="clientNonce">
        /// <para>[optional]</para>
        /// Optional client-provided nonce that must match the client_nonce value provided 
        /// during a subsequent request to the callback API.
        /// </param>
        /// <param name="mountPoint">
        /// Mount point of the JWT Auth method
        /// </param>
        /// <returns>The OIDC Auth URL</returns>
        Task<Secret<OIDCAuthURLInfo>> GetOIDCAuthURLAsync(string redirectUri, string roleName = null, string clientNonce = null, string mountPoint = AuthMethodDefaultPaths.JWT);

        /// <summary>
        /// Exchange an authorization code for an OIDC ID Token. 
        /// The ID token will be further validated against any bound claims, and if valid a Vault token will be returned.
        /// </summary>
        /// <param name="state">
        /// <para>[required]</para>
        /// Opaque state ID that is part of the Authorization URL and will be included in 
        /// the the redirect following successful authentication on the provider.
        /// </param>
        /// <param name="nonce">
        /// <para>[required]</para>
        /// Opaque nonce that is part of the Authorization URL and will be 
        /// included in the the redirect following successful authentication on the provider.
        /// </param>
        /// <param name="code">
        /// <para>[required]</para>
        /// Provider-generated authorization code that Vault will exchange for an ID token.
        /// </param>
        /// <param name="clientNonce">
        /// <para>[optional]</para>
        /// Optional client-provided nonce that must match the client_nonce value 
        /// provided during the prior request to the auth API.
        /// </param>
        /// <param name="mountPoint">
        /// Mount point of the JWT Auth method
        /// </param>
        /// <returns>The OIDC Vault Token</returns>
        Task<Secret<AuthInfo>> DoOIDCCallbackAsync(string state, string nonce, string code, string clientNonce = null, string mountPoint = AuthMethodDefaultPaths.JWT);
    }
}