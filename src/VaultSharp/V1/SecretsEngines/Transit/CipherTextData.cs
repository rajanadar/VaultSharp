using Newtonsoft.Json;

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
        [JsonProperty("ciphertext", NullValueHandling = NullValueHandling.Ignore)]
        public string CipherText { get; set; }

        [JsonProperty("key_version", NullValueHandling = NullValueHandling.Ignore)]
        public int? KeyVersion { get; set; }
    }
}