using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Details about an encryption key including metadata and encryption algorithm.
    /// </summary>
    public class EncryptionKeyInfo
    {
        /// <summary>
        /// If set, enables taking backup of named key in the plaintext format. Once set, this cannot be disabled.
        /// </summary>
        [JsonPropertyName("allow_plaintext_backup")]
        public bool AllowPlaintextBackup { get; set; }

        /// <summary>
        /// The period at which this key should be rotated automatically. 
        /// Setting this to "0" (the default) will disable automatic key rotation. 
        /// This value cannot be shorter than one hour.
        /// </summary>
        [JsonPropertyName("auto_rotate_period")]
        public long AutoRotatePeriod { get; set; }

        /// <summary>
        /// Specifies if the key is allowed to be deleted.
        /// </summary>
        [JsonPropertyName("deletion_allowed")]
        public bool DeletionAllowed { get; set; }

        /// <summary>
        /// If set, key derivation is enabled, all encrypt/decrypt requests to this named key must provide a context which is used for key derivation.
        /// </summary>
        [JsonPropertyName("derived")]
        public bool Derived { get; set; }

        /// <summary>
        /// True if the keys are to be exportable. This allows for all the valid keys in the key ring to be exported. Once set, this cannot be disabled.
        /// </summary>
        [JsonPropertyName("exportable")]
        public bool Exportable { get; set; }

        /// <summary>
        /// If set, indicates that the key is imported.
        /// </summary>
        [JsonPropertyName("imported_key")]
        public bool ImportedKey { get; set; }

        /// <summary>
        /// The list of key-version pairs in the key ring.
        /// </summary>
        [JsonPropertyName("keys")]
        public Dictionary<string, object> Keys { get; set; }

        /// <summary>
        /// The latest version of the key ring.
        /// </summary>
        [JsonPropertyName("latest_version")]
        public int LatestVersion { get; set; }

        /// <summary>
        /// The minimum available version of the key ring.
        /// </summary>
        [JsonPropertyName("min_available_version")]
        public int MinimumAvailableVersion { get; set; }

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
        /// Specifies the minimum version of the key that can be used to encrypt plaintext, sign payloads, or generate HMACs.
        /// </summary>
        [JsonPropertyName("min_encryption_version")]
        public int MinimumEncryptionVersion { get; set; }

        /// <summary>
        /// The name of the encryption key.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// If set, the key can be used in encryption requests.
        /// </summary>
        [JsonPropertyName("supports_encryption")]
        public bool SupportsEncryption { get; set; }

        /// <summary>
        /// If set, the key can be used in decryption requests.
        /// </summary>
        [JsonPropertyName("supports_decryption")]
        public bool SupportsDecryption { get; set; }

        /// <summary>
        /// If set, the key supports key derivation during encrytion requests.
        /// </summary>
        [JsonPropertyName("supports_derivation")]
        public bool SupportsDerivation { get; set; }

        /// <summary>
        /// If set, the key can be used in signing requests.
        /// </summary>
        [JsonPropertyName("supports_signing")]
        public bool SupportsSigning { get; set; }

        /// <summary>
        /// The type of key (i.e. encryption algorithm) to generate.
        /// </summary>
        [JsonPropertyName("type")]
        public TransitKeyType Type { get; set; }
    }
}