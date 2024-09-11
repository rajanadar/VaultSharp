using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the base class for audit backend options.
    /// </summary>
    public abstract class AbstractAuditBackendOptions
    {
        /// <summary>
        /// <para>[optional]</para>
        /// The elide_list_responses audit option provides the flexibility to not write the 
        /// full list response data from the audit log, to mitigate the creation of 
        /// very long individual audit records.
        /// Defaults to false.
        /// </summary>
        [JsonPropertyName("elide_list_responses")]
        public string ElideListResponses { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Indicates whether the audit device is the fallback for filtering purposes. 
        /// Vault only supports one fallback audit device at a time.
        /// Defaults to false.
        /// Enterprise only
        /// </summary>
        [JsonPropertyName("fallback")]
        public string Fallback { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Sets an optional string used to filter the audit entries logged by the audit device.
        /// Enterprise only
        /// </summary>
        [JsonPropertyName("filter")]
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the format.
        /// Allows selecting the output format. 
        /// Valid values are json (the default) and jsonx, which formats the normal log entries as XML.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        [JsonPropertyName("format")]
        public string Format { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [hmac accessor].
        /// A boolean, if set, enables the hashing of token accessor. 
        /// Defaults to true. 
        /// This option is useful only when <see cref="LogSensitiveDataInRawFormat"/> is false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [hmac accessor]; otherwise, <c>false</c>.
        /// </value>
        [JsonPropertyName("hmac_accessor")]
        public string HmacAccessor { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether [log sensitive data in raw format].
        /// Defaults to <value>"false"</value>.
        /// </summary>
        /// <value>
        /// <c>true</c> if [log sensitive data in raw format]; otherwise, <c>false</c>.
        /// </value>
        [JsonPropertyName("log_raw")]
        public string LogSensitiveDataInRawFormat { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// A customizable string prefix to write before the actual log line.
        /// </summary>
        [JsonPropertyName("prefix")]
        public string Prefix { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractAuditBackendOptions"/> class.
        /// </summary>
        protected AbstractAuditBackendOptions()
        {
            ElideListResponses = false.ToString().ToLowerInvariant();
            Fallback = string.Empty; // false.ToString().ToLowerInvariant();
            Filter = string.Empty;
            Format = "json";
            HmacAccessor = true.ToString().ToLowerInvariant();
            LogSensitiveDataInRawFormat = false.ToString().ToLowerInvariant();
            Prefix = string.Empty;
        }
    }
}