using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.GoogleCloud
{
    /// <summary>
    /// Represents the GoogleCloud OAuth2 Token.
    /// </summary>
    public class GoogleCloudOAuth2Token
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets expiry time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonProperty("expires_at_seconds")]
        public long ExpiresAtSeconds { get; set; }

        /// <summary>
        /// Gets or sets the token time to live.
        /// </summary>
        /// <value>
        /// The ttl.
        /// </value>
        [JsonProperty("token_ttl")]
        public long TokenTimeToLive { get; set; }
    }
}