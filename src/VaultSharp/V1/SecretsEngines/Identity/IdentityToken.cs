using Newtonsoft.Json;

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
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        [JsonProperty("ttl")]
        public long TimeToLive { get; set; }
    }
}