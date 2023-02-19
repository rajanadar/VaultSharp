using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents the full Secret Metadata.
    /// </summary>
    public class FullSecretMetadata
    {
        /// <summary>
        /// Gets or sets the cas required flag.
        /// </summary>
        /// <value>
        /// If true all keys will require the cas parameter to be set on all write requests.
        /// </value>
        [JsonPropertyName("cas_required")]
        public bool CASRequired { get; set; }

        /// <summary>
        /// Gets or sets the created time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonPropertyName("created_time")]
        public string CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the current version.
        /// </summary>
        /// <value>
        /// The current version.
        /// </value>
        [JsonPropertyName("current_version")]
        public int CurrentVersion { get; set; }

        /// <summary>
        /// Gets or sets the custom metadata.
        /// </summary>
        /// <value>
        /// A map of arbitrary string to string valued user-provided metadata meant to describe the secret.
        /// </value>
        [JsonPropertyName("custom_metadata")]
        public Dictionary<string, string> CustomMetadata { get; set; }

        [JsonPropertyName("delete_version_after")]
        public string DeleteVersionAfter { get; set; }

        /// <summary>
        /// Gets or sets the max version.
        /// </summary>
        /// <value>
        /// The max version.
        /// </value>
        [JsonPropertyName("max_versions")]
        public int MaxVersion { get; set; }

        /// <summary>
        /// Gets or sets the oldest version.
        /// </summary>
        /// <value>
        /// The oldest version.
        /// </value>
        [JsonPropertyName("oldest_version")]
        public int OldestVersion { get; set; }

        /// <summary>
        /// Gets or sets the deletion time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonPropertyName("updated_time")]
        public string UpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets the versions.
        /// </summary>
        /// <value>
        /// The versions.
        /// </value>
        [JsonPropertyName("versions")]
        public Dictionary<string, SecretMetadata> Versions { get; set; }
    }
}