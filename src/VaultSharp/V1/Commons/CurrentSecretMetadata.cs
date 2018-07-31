using Newtonsoft.Json;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents the current Secret Metadata.
    /// </summary>
    public class CurrentSecretMetadata : SecretMetadata
    {
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