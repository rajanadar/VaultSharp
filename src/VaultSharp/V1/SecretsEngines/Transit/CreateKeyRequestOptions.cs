using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Options used when creating new named encryption key.
    /// </summary>
    public class CreateKeyRequestOptions
    {
        /// <summary>
        /// If enabled, the key will support convergent encryption, where the same plaintext creates the same
        /// ciphertext.This requires derived to be set to true. When enabled, each encryption(/decryption/rewrap/datakey)
        /// operation will derive a nonce value rather than randomly generate it.
        /// </summary>
        [JsonProperty(PropertyName = "convergent_encryption")]
        public bool ConvergentEncryption { get; set; }

        /// <summary>
        /// Specifies if key derivation is to be used.If enabled, all encrypt/decrypt requests to this named
        /// key must provide a context which is used for key derivation.
        /// </summary>
        [JsonProperty(PropertyName = "derived")]
        public bool Derived { get; set; }

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
        /// Specifies the type of key to create.
        /// </summary>
        [JsonProperty("type")]
        public TransitKeyType Type { get; set; }

        /// <summary>
        /// The key size in bytes for algorithms that allow variable key sizes. 
        /// Currently only applicable to HMAC, where it must be between 16 and 512 bytes.
        /// </summary>
        [JsonProperty("key_size")]
        public int? KeySize { get; set; }

        /// <summary>
        /// The period at which this key should be rotated automatically. 
        /// Setting this to "0" (the default) will disable automatic key rotation. 
        /// This value cannot be shorter than one hour.
        /// </summary>
        [JsonProperty("auto_rotate_period")]
        public long AutoRotatePeriod { get; set; }

        public CreateKeyRequestOptions()
        {
            this.Type = TransitKeyType.aes256_gcm96;
        }
    }
}