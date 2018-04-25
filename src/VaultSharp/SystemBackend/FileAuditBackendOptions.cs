using Newtonsoft.Json;

namespace VaultSharp.SystemBackend
{
    /// <summary>
    /// Represents the options for the <see cref="FileAuditBackend"/>.
    /// </summary>
    public class FileAuditBackendOptions : AbstractAuditBackendOptions
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the path to where the file will be written. 
        /// If this path exists, the audit backend will append to it.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        [JsonProperty("path")]
        public string FilePath { get; set; }
    }
}