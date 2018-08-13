using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents a single Decryption item.
    /// </summary>
    public class DecryptionItem
    {
        /// <summary>
        /// [required]
        /// Specifies cipher text to be decrypted.
        /// </summary>
        [JsonProperty("ciphertext")]
        public string CipherText { get; set; }

        /// <summary>
        /// [required]
        ///  Specifies the base64 encoded context for key derivation. 
        ///  This is required if key derivation is enabled for this key.
        /// </summary>
        [JsonProperty("context")]
        public string Base64EncodedContext { get; set; }

        /// <summary>
        /// [optional]
        /// Specifies the base64 encoded nonce value. 
        /// This must be provided if convergent encryption is enabled for this key and the key was generated with Vault 0.6.1. 
        /// Not required for keys created in 0.6.2+. 
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
    }
}