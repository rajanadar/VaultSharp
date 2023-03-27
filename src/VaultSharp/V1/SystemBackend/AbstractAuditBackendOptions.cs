﻿using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the base class for audit backend options.
    /// </summary>
    public abstract class AbstractAuditBackendOptions
    {
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
        /// Initializes a new instance of the <see cref="AbstractAuditBackendOptions"/> class.
        /// </summary>
        protected AbstractAuditBackendOptions()
        {
            LogSensitiveDataInRawFormat = false.ToString().ToLowerInvariant();
            HmacAccessor = true.ToString().ToLowerInvariant();
            Format = "json";
        }
    }
}