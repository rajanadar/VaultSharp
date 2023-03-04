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
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CipherText { get; set; }

        [JsonPropertyName("key_version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? KeyVersion { get; set; }
    }
}