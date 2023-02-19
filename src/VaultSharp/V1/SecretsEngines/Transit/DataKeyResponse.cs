using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the data key response.
    /// </summary>
    public class DataKeyResponse
    {
        /// <summary>
        /// Gets or sets the cipher text.
        /// </summary>
        /// <value>
        /// The cipher text.
        /// </value>
        [JsonPropertyName("ciphertext")]
        public string CipherText { get; set; }

        [JsonPropertyName("key_version")]
        public int KeyVersion { get; set; }

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