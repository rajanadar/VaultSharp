using Newtonsoft.Json;

namespace VaultSharp.Backends.System
{
    /// <summary>
    /// Represents an audit backend.
    /// </summary>
    [JsonConverter(typeof(AuditBackendJsonConverter))]
    public abstract class AbstractAuditBackend
    {
        /// <summary>
        /// Gets or sets the mount point. If not set, the value will default to the <see cref="Type"/> value.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        [JsonIgnore]
        public string MountPoint { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        [JsonProperty("type")]
        public abstract AuditBackendType Type { get; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}