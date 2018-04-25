using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.SystemBackend
{
    /// <summary>
    /// CORS configuration.
    /// </summary>
    public class CORSConfig
    {
        /// <summary>
        /// Gets or sets a flag denoting if CORS is enabled.
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the allowed origins. 
        /// Use of wildcard '*' is allowed.
        /// </summary>
        [JsonProperty("allowed_origins")]
        public IEnumerable<string> AllowedOrigins { get; set; }

        /// <summary>
        /// Gets or sets the strings specifying headers that are permitted to be on cross-origin requests. 
        /// Headers set via this parameter will be appended to the list of headers that Vault allows by default.
        /// </summary>
        [JsonProperty("allowed_headers")]
        public IEnumerable<string> AllowedHeaders { get; set; }
    }
}