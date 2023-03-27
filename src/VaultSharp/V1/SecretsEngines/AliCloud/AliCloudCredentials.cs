using System;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("access_key")]
        public string AccessKey { get; set; }

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        [JsonPropertyName("expiration")]
        public DateTimeOffset? Expiration { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        /// <value>
        /// The secret key.
        /// </value>
        [JsonPropertyName("secret_key")]
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the STS token.
        /// </summary>
        /// <value>
        /// The secret token.
        /// </value>
        [JsonPropertyName("security_token")]
        public string SecurityToken { get; set; }
    }
}