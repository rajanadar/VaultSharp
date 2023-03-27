using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the response for a request to hash a data string.
    /// </summary>
    public class HashResponse
    {
        /// <summary>
        /// Gets or sets the hash sum using the output encoding specified on the request.
        /// </summary>
        /// <value>The hash sum.</value>
        [JsonPropertyName("sum")]
        public string HashSum { get; set; }
    }
}