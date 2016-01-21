using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.Transit
{
    /// <summary>
    /// Represents the transit key data.
    /// </summary>
    public class TransitKeyData
    {
        /// <summary>
        /// Gets or sets the plain text key.
        /// </summary>
        /// <value>
        /// The plain text key.
        /// </value>
        [JsonProperty("plaintext")]
        public string PlainTextKey { get; set; }

        /// <summary>
        /// Gets or sets the cipher text key.
        /// </summary>
        /// <value>
        /// The cipher text key.
        /// </value>
        [JsonProperty("ciphertext")]
        public string CipherTextKey { get; set; }
    }
}