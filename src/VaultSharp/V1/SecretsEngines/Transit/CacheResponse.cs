using System.Text.Json.Serialization;

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
        [JsonPropertyName("size")]
        public int Size { get; set; }
    }
}