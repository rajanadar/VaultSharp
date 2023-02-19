using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{ 
    /// <summary>
    /// Represents the options for the <see cref="SyslogAuditBackend"/>.
    /// </summary>
    public class SyslogAuditBackendOptions : AbstractAuditBackendOptions
    {
        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the syslog facility to use. 
        /// Defaults to "AUTH".
        /// </summary>
        /// <value>
        /// The facility.
        /// </value>
        [JsonPropertyName("facility")]
        public string Facility { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the syslog tag to use.
        /// Defaults to "vault".
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        [JsonPropertyName("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogAuditBackendOptions"/> class.
        /// </summary>
        public SyslogAuditBackendOptions()
        {
            Facility = "AUTH";
            Tag = "vault";
        }
    }
}