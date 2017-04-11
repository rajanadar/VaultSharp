using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.Audit.Models.Custom
{
    /// <summary>
    /// Represents a custom <see cref="AuditBackendType"/> based audit backend.
    /// </summary>
    public class CustomAuditBackend : AuditBackend
    {
        private readonly AuditBackendType _auditBackendType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuditBackend"/> class.
        /// </summary>
        /// <param name="auditBackendType">Type of the audit backend.</param>
        public CustomAuditBackend(AuditBackendType auditBackendType)
        {
            _auditBackendType = auditBackendType;
        }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the generic options. Use any key/value pairs suitable for the custom audit backend.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        [JsonProperty("options")]
        public Dictionary<string, string> Options { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        public override AuditBackendType BackendType { get { return _auditBackendType; } }
    }
}