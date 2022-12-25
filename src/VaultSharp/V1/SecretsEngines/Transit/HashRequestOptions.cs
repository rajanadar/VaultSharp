
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the options for a request to hash
    /// </summary>
    public class HashRequestOptions
    {
        [JsonProperty("algorithm")]
        public TransitHashAlgorithm Algorithm { get; set; } = TransitHashAlgorithm.SHA2_256;

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
        public OutputEncodingFormat Format { get; set; }
    }
}