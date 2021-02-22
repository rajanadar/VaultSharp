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
        ///  Specifies the version of the key to use for encryption. 
        ///  If not set, uses the latest version. 
        ///  Must be greater than or equal to the key's min_encryption_version, if set.
        /// </summary>
        [JsonProperty("key_version")]
        public int? KeyVersion { get; set; }

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

        /// <summary>
        /// [required/optional]
        /// This parameter is required when encryption key is expected to be created. 
        /// When performing an upsert operation, the type of key to create.
        /// </summary>
        [JsonProperty("type")]
        public TransitKeyType KeyType { get; set; }

        /// <summary>
        /// This parameter will only be used when a key is expected to be created. 
        /// Whether to support convergent encryption. 
        /// This is only supported when using a key with key derivation enabled and will require all requests to carry both a 
        /// context and 96-bit (12-byte) nonce. 
        /// The given nonce will be used in place of a randomly generated nonce. 
        /// As a result, when the same context and nonce are supplied, the same ciphertext is generated. 
        /// It is very important when using this mode that you ensure that all nonces are unique for a given context. 
        /// Failing to do so will severely impact the ciphertext's security.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [convergent encryption]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("convergent_encryption")]
        public bool ConvergentEncryption { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="EncryptionItem"/>.
        /// </summary>
        public EncryptionItem()
        {
            KeyType = TransitKeyType.aes256_gcm96;
        }
    }
}