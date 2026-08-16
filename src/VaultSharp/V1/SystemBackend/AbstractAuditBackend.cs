using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
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
        [JsonPropertyName("type")]
        public abstract AuditBackendType Type { get; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a flag indicating if this is a local mount.
        /// </summary>
        /// <remarks>
        /// The option is allowed in Vault open-source, but relevant functionality is only supported in Vault Enterprise:
        /// </remarks>
        /// <value>
        /// The flag.
        /// </value>
        [JsonPropertyName("local")]
        public bool Local { get; set; }
    }
}