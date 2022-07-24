using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the options for a request to hash
    /// </summary>
    public class HashRequestOptions
    {
        /// <summary>
        /// Gets or sets the base64 encoded input data to be hashed.
        /// </summary>
        /// <value>The base64 encoded input data to be hashed.</value>
        [JsonProperty("input")]
        public string Base64EncodedInput { get; set; }


        /// <summary>
        /// Gets or sets the output encoding for the response.
        /// </summary>
        /// <value>The output encoding for the response.</value>
        [JsonProperty("format")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OutputEncodingFormat Format { get; set; }
    }
}