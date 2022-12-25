using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Options used when importing named encryption key.
    /// </summary>
    public class ImportKeyRequestOptions
    {
        /// <summary>
        /// A base64-encoded string that contains two values: 
        /// an ephemeral 256-bit AES key wrapped using the wrapping key returned by Vault 
        /// and the encryption of the import key material under the provided AES key. 
        /// The wrapped AES key should be the first 512 bytes of the ciphertext, 
        /// and the encrypted key material should be the remaining bytes. 
        /// </summary>
        [JsonProperty(PropertyName = "ciphertext")]
        public string Base64EncodedCipherText { get; set; }

        /// <summary>
        /// The hash function used for the RSA-OAEP step of creating the ciphertext.
        /// </summary>
        [JsonProperty(PropertyName = "hash_function")]
        public TransitHashFunction HashFunction { get; set; } = TransitHashFunction.SHA256;

        /// <summary>
        /// Specifies the type of key to create.
        /// </summary>
        [JsonProperty("type")]
        public TransitKeyType Type { get; set; }

        /// <summary>
        /// If set, the imported key can be rotated within Vault by using the rotate endpoint.
        /// </summary>
        [JsonProperty(PropertyName = "allow_rotation")]
        public bool AllowRotation { get; set; }

        /// <summary>
        /// Specifies if key derivation is to be used.If enabled, all encrypt/decrypt requests to this named
        /// key must provide a context which is used for key derivation.
        /// </summary>
        [JsonProperty(PropertyName = "derived")]
        public bool Derived { get; set; }

        /// <summary>
        /// A base64-encoded string providing a context for key derivation. 
        /// Required if derived is set to true.
        /// </summary>
        [JsonProperty(PropertyName = "context")]
        public string Base64EncodedKeyDerivationContext { get; set; }

        /// <summary>
        /// Enables keys to be exportable. This allows for all the valid keys in the key ring to be
        /// exported. Once set, this cannot be disabled.
        /// </summary>
        [JsonProperty(PropertyName = "exportable")]
        public bool Exportable { get; set; }

        /// <summary>
        /// If set, enables taking backup of named key in the plaintext format.Once set, this cannot be disabled.
        /// </summary>
        [JsonProperty(PropertyName = "allow_plaintext_backup")]
        public bool AllowPlaintextBackup { get; set; }

        /// <summary>
        /// The period at which this key should be rotated automatically. 
        /// Setting this to "0" (the default) will disable automatic key rotation. 
        /// This value cannot be shorter than one hour.
        /// </summary>
        [JsonProperty("auto_rotate_period")]
        public long AutoRotatePeriod { get; set; }
    }
}