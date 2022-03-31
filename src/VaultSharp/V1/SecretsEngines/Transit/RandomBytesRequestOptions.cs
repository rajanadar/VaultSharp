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
        public OutputEncodingFormat Format { get; set; }
    }
}