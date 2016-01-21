using Newtonsoft.Json;

namespace VaultSharp.Backends.Audit.Models.File
{
    /// <summary>
    /// Represents the <see cref="AuditBackendType.File"/> based audit backend.
    /// This audit backend writes audit logs to a file.
    /// This is a very simple audit backend: it appends logs to a file.
    /// It does not currently assist with any log rotation.
    /// </summary>
    public class FileAuditBackend : AuditBackend
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        [JsonProperty("options")]
        public FileAuditBackendOptions Options { get; set; }
    }
}