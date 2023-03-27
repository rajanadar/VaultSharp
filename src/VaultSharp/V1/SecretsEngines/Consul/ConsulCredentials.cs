using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Consul
{
    /// <summary>
    /// Represents the Consul credentials.
    /// </summary>
    public class ConsulCredentials
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}