using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the Encrypt Request Options.
    /// </summary>
    public class EncryptRequestOptions : EncryptionItem
    {
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
        /// Specifies a list of items to be encrypted in a single batch. 
        /// When this parameter is set, if the parameters 'plaintext', 'context' and 'nonce' are also set, they will be ignored.
        /// </summary>
        [JsonProperty("batch_input")]
        public List<EncryptionItem> BatchedEncryptionItems { get; set; }

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
        /// Initializes a new instance of <see cref="EncryptRequestOptions"/>.
        /// </summary>
        public EncryptRequestOptions()
        {
            KeyType = TransitKeyType.aes256_gcm96;
        }
    }
}