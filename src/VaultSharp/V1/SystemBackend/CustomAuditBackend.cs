using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents a custom <see cref="AuditBackendType"/> based audit backend.
    /// </summary>
    public class CustomAuditBackend : AbstractAuditBackend
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuditBackend"/> class.
        /// </summary>
        /// <param name="auditBackendType">Type of the audit backend.</param>
        public CustomAuditBackend(AuditBackendType auditBackendType)
        {
            Type = auditBackendType;
        }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the generic options. Use any key/value pairs suitable for the custom audit backend.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        [JsonPropertyName("options")]
        public Dictionary<string, string> Options { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the type of the backend.
        /// </summary>
        /// <value>
        /// The type of the backend.
        /// </value>
        [JsonPropertyName("type")]
        public override AuditBackendType Type { get; }
    }
}