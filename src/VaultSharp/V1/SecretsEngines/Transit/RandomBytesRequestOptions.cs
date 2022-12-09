using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the options for a request for Vault to return a set of random bytes.
    /// </summary>
    public class RandomBytesRequestOptions
    {
        /// <summary>
        /// Gets or sets the format to return the bytes in.
        /// </summary>
        /// <value>The format to return the bytes in.</value>
        [JsonProperty("format")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OutputEncodingFormat Format { get; set; } = OutputEncodingFormat.base64;

        /// <summary>
        /// Gets or sets the source of the requested bytes. 
        /// platform, the default, sources bytes from the platform's entropy source. 
        /// seal sources from entropy augmentation (enterprise only). 
        /// all mixes bytes from all available sources.
        /// </summary>
        /// <value>The entropy source for random bytes.</value>
        [JsonProperty("source")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RandomBytesSource Source { get; set; } = RandomBytesSource.platform;
    }
}