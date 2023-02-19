using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the Cipher text data.
    /// </summary>
    public class CipherTextData
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
        public int? KeyVersion { get; set; }
    }
}