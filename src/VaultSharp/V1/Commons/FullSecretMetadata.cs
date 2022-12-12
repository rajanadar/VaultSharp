using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents the full Secret Metadata.
    /// </summary>
    public class FullSecretMetadata
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
        /// Gets or sets the current version.
        /// </summary>
        /// <value>
        /// The current version.
        /// </value>
        [JsonProperty("current_version")]
        public int CurrentVersion { get; set; }

        /// <summary>
        /// Gets or sets the max version.
        /// </summary>
        /// <value>
        /// The max version.
        /// </value>
        [JsonProperty("max_version")]
        public int MaxVersion { get; set; }

        /// <summary>
        /// Gets or sets the oldest version.
        /// </summary>
        /// <value>
        /// The oldest version.
        /// </value>
        [JsonProperty("oldest_version")]
        public int OldestVersion { get; set; }

        /// <summary>
        /// Gets or sets the deletion time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [JsonProperty("updated_time")]
        public string UpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets the versions.
        /// </summary>
        /// <value>
        /// The versions.
        /// </value>
        [JsonProperty("versions")]
        public Dictionary<string, SecretMetadata> Versions { get; set; }

        /// <summary>
        /// Gets or sets the cas required flag.
        /// </summary>
        /// <value>
        /// If true all keys will require the cas parameter to be set on all write requests.
        /// </value>
        [JsonProperty("cas_required")]
        public bool CASRequired { get; set; }

        /// <summary>
        /// Gets or sets the custom metadata.
        /// </summary>
        /// <value>
        /// A map of arbitrary string to string valued user-provided metadata meant to describe the secret.
        /// </value>
        [JsonProperty("custom_metadata")]
        public Dictionary<string, string> CustomMetadata { get; set; }
    }
}