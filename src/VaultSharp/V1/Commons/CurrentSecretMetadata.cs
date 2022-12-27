using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty("created_time")]
        public string CreatedTime { get; set; }

        [JsonProperty("custom_metadata")]
        public Dictionary<string, object> CustomMetadata { get; set; }

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

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonProperty("version")]
        public int Version { get; set; }
    }
}