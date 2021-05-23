using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.GoogleCloud
{
    /// <summary>
    /// GoogleCloud Secrets Engine.
    /// </summary>
    public interface IGoogleCloudSecretsEngine
    {
        /// <summary>
        /// Generates an OAuth2 token with the scopes defined on the roleset. 
        /// This OAuth access token can be used in GCP API calls.
        /// Tokens are non-renewable and have a TTL of an hour by default.
        /// </summary>
        /// <param name="roleset"><para>[required]</para>
        /// Name of an roleset with secret type access_token to generate access_token under.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.GoogleCloud" />
        /// Provide a value only if you have customized the Consul mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="GoogleCloudOAuth2Token" /> as the data.
        /// </returns>
        Task<Secret<GoogleCloudOAuth2Token>> GetOAuth2TokenAsync(string roleset, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Generates a service account key.
        /// These keys are renewable and have TTL/max TTL as defined by either the backend config or the 
        /// system default if config was not defined.
        /// </summary>
        /// <param name="roleset"><para>[required]</para>
        /// Name of an roleset with secret type service_account_key to generate key under.
        /// </param>
        /// <param name="keyAlgorithm"><para>[optional]</para>
        /// Key algorithm used to generate key. 
        /// Defaults to 2k RSA key You probably should not choose other values (i.e. 1k).
        /// </param>
        /// <param name="privateKeyType"><para>[optional]</para>
        ///  Private key type to generate. Defaults to JSON credentials file. 
        /// </param>
        /// <param name="timeToLive"><para>[optional]</para>
        /// Specifies the Time To Live value provided as a string duration with time suffix. 
        /// If not set, uses the system default value.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.GoogleCloud" />
        /// Provide a value only if you have customized the Consul mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="GoogleCloudOAuth2Token" /> as the data.
        /// </returns>
        Task<Secret<GoogleCloudServiceAccountKey>> GetServiceAccountKeyAsync(string roleset, ServiceAccountKeyAlgorithm keyAlgorithm = ServiceAccountKeyAlgorithm.KEY_ALG_RSA_2048, ServiceAccountPrivateKeyType privateKeyType = ServiceAccountPrivateKeyType.TYPE_GOOGLE_CREDENTIALS_FILE, string timeToLive = "",  string mountPoint = null, string wrapTimeToLive = null);
    }
}