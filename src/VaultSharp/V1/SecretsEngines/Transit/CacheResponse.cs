using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the cache configuration response.
    /// </summary>
    public class CacheResponse
    {
        /// <summary>
        /// Gets or sets the size of the cache, in terms of the number of entries.
        /// </summary>
        /// <value>The size of the cache, in terms of the number of entries.</value>
        [JsonProperty("size")]
        public uint Size { get; set; }
    }
}