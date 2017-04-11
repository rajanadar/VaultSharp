using Newtonsoft.Json;

namespace VaultSharp.Backends.Audit.Models.Syslog
{
    /// <summary>
    /// Represents the <see cref="AuditBackendType.Syslog"/> based audit backend.
    /// This audit backend writes audit logs to syslog.
    /// It currently does not support a configurable syslog destination, and always sends to the local agent.
    /// This backend is only supported on Unix systems, and should not be enabled if any standby Vault instances do not support it.
    /// </summary>
    public class SyslogAuditBackend : AuditBackend
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        public override AuditBackendType BackendType
        {
            get
            {
                return AuditBackendType.Syslog;
            }
        }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        [JsonProperty("options")]
        public SyslogAuditBackendOptions Options { get; set; }
    }
}