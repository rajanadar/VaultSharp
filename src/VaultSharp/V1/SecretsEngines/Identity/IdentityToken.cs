using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Represents the IdentityToken
    /// </summary>
    public class IdentityToken
    {
        /// <summary>
        /// Gets or sets the Client Id.
        /// </summary>
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        [JsonPropertyName("ttl")]
        public int TimeToLive { get; set; }
    }
}