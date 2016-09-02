using Newtonsoft.Json;

namespace VaultSharp.Backends.Audit.Models.File
{
    /// <summary>
    /// Represents the options for the <see cref="FileAuditBackend"/>.
    /// </summary>
    public class FileAuditBackendOptions
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

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the flag indicating if security sensitive information be logged raw. 
        /// Defaults to <value>"false"</value>.
        /// </summary>
        /// <value>
        /// The boolean value.
        /// </value>
        [JsonProperty("log_raw")]
        public string LogSensitiveDataInRawFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAuditBackendOptions"/> class.
        /// </summary>
        public FileAuditBackendOptions()
        {
            LogSensitiveDataInRawFormat = false.ToString().ToLowerInvariant();
        }
    }
}