using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Options for configuring the transit engine's cache.
    /// </summary>
    public class CacheConfigRequestOptions
    {
        /// <summary>
        /// Gets or sets the size, in terms of number of entries.
        /// </summary>
        /// <remarks>Must be 0 (default) or a value greater than or equal to 10 (minimum cache size).</remarks>
        /// <value>The size, in terms of number of entries.</value>
        [JsonProperty("size")]
        public uint Size { get; set; }

    }
}