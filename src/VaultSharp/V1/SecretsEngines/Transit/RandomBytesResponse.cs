using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the response for a request to return random bytes.
    /// </summary>
    public class RandomBytesResponse
    {
        /// <summary>
        /// Gets or sets the random bytes, returned in the format found on the request.
        /// </summary>
        /// <value>The encoded random bytes.</value>
        [JsonPropertyName("random_bytes")]
        public string EncodedRandomBytes { get; set; }
    }
}