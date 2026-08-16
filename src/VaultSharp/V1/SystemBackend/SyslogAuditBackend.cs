using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the <see cref="AuditBackendType.Syslog"/> based audit backend.
    /// This audit backend writes audit logs to syslog.
    /// It currently does not support a configurable syslog destination, and always sends to the local agent.
    /// This backend is only supported on Unix systems, and should not be enabled if any standby Vault instances do not support it.
    /// </summary>
    public class SyslogAuditBackend : AbstractAuditBackend
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        [JsonPropertyName("type")]
        public override AuditBackendType Type { get; } = AuditBackendType.Syslog;

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        [JsonPropertyName("options")]
        public SyslogAuditBackendOptions Options { get; set; }
    }
}