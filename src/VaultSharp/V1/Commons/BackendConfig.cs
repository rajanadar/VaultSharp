using Newtonsoft.Json;

namespace VaultSharp.Core
{
    /// <summary>
    /// Represents the configuration values for a backend.
    /// </summary>
    public class BackendConfig
    {
        /// <summary>
        /// Gets or sets the default lease TTL.
        /// A value of "0" means that the system defaults are used by this backend.
        /// </summary>
        /// <value>
        /// The default lease TTL.
        /// </value>
        [JsonProperty("default_lease_ttl")]
        public long DefaultLeaseTtl { get; set; }

        /// <summary>
        /// Gets or sets the cache flag.
        /// </summary>
        /// <value>
        /// The cache flag.
        /// </value>
        [JsonProperty("force_no_cache")]
        public bool ForceNoCache { get; set; }

        /// <summary>
        /// Gets or sets the maximum lease TTL.
        /// A value of "0" means that the system defaults are used by this backend.
        /// </summary>
        /// <value>
        /// The maximum lease TTL.
        /// </value>
        [JsonProperty("max_lease_ttl")]
        public long MaximumLeaseTtl { get; set; }

        // raja todo: this sounds non-generic.

        /// <summary>
        /// Gets or sets the token type.
        /// </summary>
        /// <value>
        /// The token type.
        /// </value>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the plugin name.
        /// </summary>
        /// <value>
        /// The plugin name.
        /// </value>
        [JsonProperty("plugin_name")]
        public string PluginName { get; set; }
    }
}