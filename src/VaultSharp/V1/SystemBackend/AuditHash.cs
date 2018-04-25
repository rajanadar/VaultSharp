using Newtonsoft.Json;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the Audit hash.
    /// </summary>
    public class AuditHash
    {
        /// <summary>
        /// Gets or sets a the hash.
        /// </summary>
        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}