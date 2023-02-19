using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// CORS configuration.
    /// </summary>
    public class CORSConfig
    {
        /// <summary>
        /// Gets or sets a flag denoting if CORS is enabled.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the allowed origins. 
        /// Use of wildcard '*' is allowed.
        /// </summary>
        [JsonPropertyName("allowed_origins")]
        public IEnumerable<string> AllowedOrigins { get; set; }

        /// <summary>
        /// Gets or sets the strings specifying headers that are permitted to be on cross-origin requests. 
        /// Headers set via this parameter will be appended to the list of headers that Vault allows by default.
        /// </summary>
        [JsonPropertyName("allowed_headers")]
        public IEnumerable<string> AllowedHeaders { get; set; }
    }
}