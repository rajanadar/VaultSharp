using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the expiration data.
    /// </summary>
    public class ExpiryData
    {
        /// <summary>
        /// Gets or sets the expiry.
        /// </summary>
        /// <value>
        /// The expiry.
        /// </value>
        [JsonProperty("expiry")]
        public string Expiry { get; set; }
    }
}