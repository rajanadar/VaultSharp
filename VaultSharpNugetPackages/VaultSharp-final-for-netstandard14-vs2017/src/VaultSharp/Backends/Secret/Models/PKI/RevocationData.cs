using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the Revocation data.
    /// </summary>
    public class RevocationData
    {
        /// <summary>
        /// Gets or sets the revocation time.
        /// </summary>
        /// <value>
        /// The revocation time.
        /// </value>
        [JsonProperty("revocation_time")]
        public int RevocationTime { get; set; }
    }
}