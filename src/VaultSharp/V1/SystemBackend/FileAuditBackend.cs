using Newtonsoft.Json;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the <see cref="AuditBackendType.File"/> based audit backend.
    /// This audit backend writes audit logs to a file.
    /// This is a very simple audit backend: it appends logs to a file.
    /// It does not currently assist with any log rotation.
    /// </summary>
    public class FileAuditBackend : AbstractAuditBackend
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        public override AuditBackendType Type { get; } = AuditBackendType.File;

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        [JsonProperty("options")]
        public FileAuditBackendOptions Options { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        [JsonProperty("path")]
        public string Path { get; set; }
    }
}