using System;
using System.Net.Http;
using VaultSharp.Backends.Authentication.Models;

namespace VaultSharp
{
    /// <summary>
    /// Represents a set of methods for creating instances of the <see cref="IVaultClient" /> interface implementations.
    /// </summary>
    public static class VaultClientFactory
    {
        /// <summary>
        /// Creates an instance of the vault client, with the provided <see cref="IAuthenticationInfo" /> used to authenticate and authorize the user.
        /// This is the typical client you would need for your consuming applications.
        /// <para>
        /// If you need an instance of an administrative/root user based <see cref="IVaultClient" />, pass a <see cref="IAuthenticationInfo" /> with a root policy mapping.
        /// If you need an instance of an unauthenticated <see cref="IVaultClient" />, pass a <value>null</value> value for <see cref="IAuthenticationInfo" />.
        /// An unauthenticated client can do very few operations. e.g. Check seal status, initialization status etc.
        /// </para><para>var vaultClient = VaultClientFactory.CreateVaultClient(new Uri("http://127.0.0.1:8200", new GitHubAuthenticationInfo(personalAccessToken: "YOUR_TOKEN"));</para><para>var administrativeVaultClient = VaultClientFactory.CreateVaultClient(new Uri("http://127.0.0.1:8200", new TokenAuthenticationInfo(token: "ROOT_POLICY_TOKEN"));</para><para>var unauthenticatedVaultClient = VaultClientFactory.CreateVaultClient(new Uri("http://127.0.0.1:8200", authenticationInfo: null));</para>
        /// </summary>
        /// <param name="vaultServerUriWithPort"><para>[required]</para>
        /// The vault server URI with port.</param>
        /// <param name="authenticationInfo"><para>[optional]</para>
        /// The authentication information. e.g. GitHub, AppId, LDAP etc.</param>
        /// <param name="continueAsyncTasksOnCapturedContext"><para>[optional]</para>
        /// if set to <c>true</c> [continue asynchronous tasks on captured context].</param>
        /// <param name="serviceTimeout"><para>[optional]</para>
        /// The timeout value for the Vault Service calls. Do not specify a value, if you want to go with the default timeout values.</param>
        /// <param name="postHttpClientInitializeAction"><para>[optional]</para>
        /// A post-processing delegate on the <see cref="HttpClient"/> instance used by the library.
        /// This can be used to setup any custom message handlers, proxy settings etc.
        /// Please note that the delegate will get an instance of <see cref="HttpClient"/> that is initialized with the address
        /// and timeout settings.
        /// </param>
        /// <returns>
        /// An instance of the <see cref="IVaultClient" /> interface implementation.
        /// </returns>
        public static IVaultClient CreateVaultClient(Uri vaultServerUriWithPort, IAuthenticationInfo authenticationInfo, bool continueAsyncTasksOnCapturedContext = false, TimeSpan? serviceTimeout = null, Action<HttpClient> postHttpClientInitializeAction = null)
        {
            IVaultClient vaultClient = new VaultClient(vaultServerUriWithPort, authenticationInfo, continueAsyncTasksOnCapturedContext, serviceTimeout, postHttpClientInitializeAction: postHttpClientInitializeAction);
            return vaultClient;
        }
    }
}