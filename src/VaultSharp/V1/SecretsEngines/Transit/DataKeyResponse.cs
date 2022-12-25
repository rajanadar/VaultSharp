using Newtonsoft.Json;

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
        [JsonProperty("ciphertext")]
        public string CipherText { get; set; }

        [JsonProperty("key_version")]
        public int KeyVersion { get; set; }

        /// <summary>
        /// Gets or sets the plain text.
        /// </summary>
        /// <value>
        /// The plain text.
        /// </value>
        [JsonProperty("plaintext")]
        public string Base64EncodedPlainText { get; set; }
    }
}