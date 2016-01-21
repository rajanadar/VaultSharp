using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.Consul
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
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}