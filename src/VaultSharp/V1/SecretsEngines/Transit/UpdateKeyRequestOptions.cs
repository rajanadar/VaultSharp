using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents that options that will be updated on the encryption key. Only the options
    /// set are sent on the request. Unset field will not be serialized on the request payload.
    /// </summary>
    public class UpdateKeyRequestOptions
    {
        /// <summary>
        /// Specifies if the key is allowed to be deleted.
        /// </summary>
        [JsonPropertyName("deletion_allowed")]
        public bool DeletionAllowed { get; set; }

        /// <summary>
        /// Enables keys to be exportable. This allows for all the valid keys in the key ring to be
        /// exported. Once set, this cannot be disabled.
        /// </summary>
        [JsonPropertyName("exportable")]
        public bool Exportable { get; set; }

        /// <summary>
        /// Enables taking backup of named key in the plaintext format. Once set, this cannot be disabled.
        /// </summary>
        [JsonPropertyName("allow_plaintext_backup")]
        public bool AllowPlaintextBackup { get; set; }

        /// <summary>
        /// Specifies the minimum version of ciphertext allowed to be decrypted. Adjusting this as part
        /// of a key rotation policy can prevent old copies of ciphertext from being decrypted, should 
        /// they fall into the wrong hands. For signatures, this value controls the minimum version of
        /// signature that can be verified against. For HMACs, this controls the minimum version of a
        /// key allowed to be used as the key for verification.
        /// </summary>
        [JsonPropertyName("min_decryption_version")]
        public int MinimumDecryptionVersion { get; set; }

        /// <summary>
        /// Specifies the minimum version of the key that can be used to encrypt plaintext, sign payloads,
        /// or generate HMACs. Must be 0 (which will use the latest version) or a value greater or equal
        /// to min_decryption_version.
        /// </summary>
        [JsonPropertyName("min_encryption_version")]
        public int MinimumEncryptionVersion { get; set; }

        /// <summary>
        /// The period at which this key should be rotated automatically. 
        /// Setting this to "0" (the default) will disable automatic key rotation. 
        /// This value cannot be shorter than one hour.
        /// </summary>
        [JsonPropertyName("auto_rotate_period")]
        public long AutoRotatePeriod { get; set; }
    }
}