using System.Text.Json.Serialization;

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
        [JsonPropertyName("created_time")]
        public string CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the deletion time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonPropertyName("deletion_time")]
        public string DeletionTime { get; set; }

        /// <summary>
        /// Gets or sets the destroyed flag.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        [JsonPropertyName("destroyed")]
        public bool Destroyed { get; set; }
    }
}