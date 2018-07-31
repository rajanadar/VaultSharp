using Newtonsoft.Json;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents a Vault Secret Metadata.
    /// </summary>
    public class SecretMetadata
    {
        /// <summary>
        /// Gets or sets the created time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonProperty("created_time")]
        public string CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the deletion time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonProperty("deletion_time")]
        public string DeletionTime { get; set; }

        /// <summary>
        /// Gets or sets the destroyed flag.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        [JsonProperty("destroyed")]
        public bool Destroyed { get; set; }
    }
}