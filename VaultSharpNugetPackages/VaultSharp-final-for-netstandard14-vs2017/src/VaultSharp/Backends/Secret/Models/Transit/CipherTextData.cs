using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.Transit
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
        [JsonProperty("ciphertext")]
        public string CipherText { get; set; }
    }
}