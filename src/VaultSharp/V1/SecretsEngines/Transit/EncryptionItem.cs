using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents a single Encryption item.
    /// </summary>
    public class EncryptionItem
    {
        /// <summary>
        /// [required]
        /// Specifies base64 encoded plaintext to be encrypted.
        /// </summary>
        [JsonProperty("plaintext")]
        public string Base64EncodedPlainText { get; set; }

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
        /// The value must be exactly 96 bits (12 bytes) long and the user must ensure that for any given context 
        /// (and thus, any given encryption key) this nonce value is never reused.
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
    }
}