using System;
using System.Net.Http;
using VaultSharp.V1.AuthMethods;

namespace VaultSharp
{
    /// <summary>
    /// The vault client settings.
    /// </summary>
    public class VaultClientSettings
    {
        /// <summary>
        /// Constructor with bare minimal required fields.
        /// </summary>
        /// <param name="vaultServerUriWithPort"></param>
        /// <param name="authMethodInfo"></param>
        public VaultClientSettings(string vaultServerUriWithPort, IAuthMethodInfo authMethodInfo)
        {
            VaultServerUriWithPort = vaultServerUriWithPort;
            AuthMethodInfo = authMethodInfo;
        }

        /// <summary>
        /// The Vault Server Uri with port.
        /// </summary>
        public string VaultServerUriWithPort { get; }

        /// <summary>
        /// The auth method to be used to acquire a vault token.
        /// </summary>
        public IAuthMethodInfo AuthMethodInfo { get; }

        /// <summary>
        /// Flag to indicate async context.
        /// </summary>
        public bool ContinueAsyncTasksOnCapturedContext { get; set; }

        /// <summary>
        /// The Api timeout.
        /// </summary>
        public TimeSpan? VaultServiceTimeout { get; set; }

        /// <summary>
        /// The one time http client's http client handler delegate.
        /// Use this to set proxy settings etc.
        /// </summary>
        public Action<HttpClientHandler> PostProcessHttpClientHandlerAction { get; set; }

        /// <summary>
        /// The per http request delegate invoked before every vault api http request.
        /// </summary>
        public Action<HttpClient, HttpRequestMessage> BeforeApiRequestAction { get; set; }

        /// <summary>
        /// The per http response delegate invoked after every vault api http response.
        /// </summary>
        public Action<HttpResponseMessage> AfterApiResponseAction { get; set; }

        /// <summary>
        /// Flag to indicate how the vault token should be passed to the API.
        /// Default is to use the Authorization: Bearer &lt;vault-token&gt; scheme.
        /// </summary>
        public bool UseVaultTokenHeaderInsteadOfAuthorizationHeader { get; set; }

        /// <summary>
        /// The namespace to use to achieve tenant level isolation.
        /// Enterprise Vault only.
        /// </summary>
        public string Namespace { get; set; }
    }
}
