using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents the current Secret Metadata.
    /// </summary>
    public class CurrentSecretMetadata
    {
        /// <summary>
        /// Gets or sets the created time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonPropertyName("created_time")]
        public string CreatedTime { get; set; }

        [JsonPropertyName("custom_metadata")]
        public Dictionary<string, object> CustomMetadata { get; set; }

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

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonPropertyName("version")]
        public int Version { get; set; }
    }
}