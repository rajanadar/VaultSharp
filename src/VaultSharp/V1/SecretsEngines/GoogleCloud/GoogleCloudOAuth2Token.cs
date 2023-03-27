using System.Text.Json.Serialization;

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
        [JsonPropertyName("token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets expiry time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonPropertyName("expires_at_seconds")]
        public long ExpiresAtSeconds { get; set; }

        /// <summary>
        /// Gets or sets the token time to live.
        /// </summary>
        /// <value>
        /// The ttl.
        /// </value>
        [JsonPropertyName("token_ttl")]
        public int TokenTimeToLive { get; set; }
    }
}