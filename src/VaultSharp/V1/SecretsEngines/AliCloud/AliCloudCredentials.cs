using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AliCloud
{
    /// <summary>
    /// Represents the AliCloud credentials.
    /// </summary>
    public class AliCloudCredentials
    {
        /// <summary>
        /// Gets or sets the access key.
        /// </summary>
        /// <value>
        /// The access key.
        /// </value>
        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        /// <value>
        /// The secret key.
        /// </value>
        [JsonProperty("secret_key")]
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the STS token.
        /// </summary>
        /// <value>
        /// The secret token.
        /// </value>
        [JsonProperty("security_token")]
        public string SecurityToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        [JsonProperty("expiration")]
        public string Expiration { get; set; }
    }
}