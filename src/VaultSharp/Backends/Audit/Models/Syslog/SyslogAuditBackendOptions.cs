using Newtonsoft.Json;

namespace VaultSharp.Backends.Audit.Models.Syslog
{
    /// <summary>
    /// Represents the options for the <see cref="SyslogAuditBackend"/>.
    /// </summary>
    public class SyslogAuditBackendOptions
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the syslog facility to use. 
        /// Defaults to "AUTH".
        /// </summary>
        /// <value>
        /// The facility.
        /// </value>
        [JsonProperty("facility")]
        public string Facility { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the syslog tag to use.
        /// Defaults to "vault".
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if security sensitive information be logged raw. 
        /// Defaults to <value>"false"</value>.
        /// </summary>
        /// <value>
        /// The boolean value.
        /// </value>
        [JsonProperty("log_raw")]
        public string LogSensitiveDataInRawFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogAuditBackendOptions"/> class.
        /// </summary>
        public SyslogAuditBackendOptions()
        {
            Facility = "AUTH";
            Tag = "vault";
            LogSensitiveDataInRawFormat = false.ToString().ToLowerInvariant();
        }
    }
}