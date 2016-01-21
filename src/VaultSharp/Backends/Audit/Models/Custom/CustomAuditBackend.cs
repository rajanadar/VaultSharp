using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.Audit.Models.Custom
{
    /// <summary>
    /// Represents a custom <see cref="AuditBackendType"/> based audit backend.
    /// </summary>
    public class CustomAuditBackend : AuditBackend
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the generic options. Use any key/value pairs suitable for the custom audit backend.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        [JsonProperty("options")]
        public Dictionary<string, string> Options { get; set; }
    }
}