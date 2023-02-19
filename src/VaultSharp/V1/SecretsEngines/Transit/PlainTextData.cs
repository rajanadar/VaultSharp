using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the Plain text data.
    /// </summary>
    public class PlainTextData
    {
        /// <summary>
        /// Gets or sets the plain text.
        /// </summary>
        /// <value>
        /// The plain text.
        /// </value>
        [JsonPropertyName("plaintext")]
        public string Base64EncodedPlainText { get; set; }
    }
}