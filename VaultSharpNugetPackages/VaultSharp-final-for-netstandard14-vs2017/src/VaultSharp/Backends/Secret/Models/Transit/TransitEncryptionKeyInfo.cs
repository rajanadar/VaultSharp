using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.Transit
{
    /// <summary>
    /// Represents the encryption key information.
    /// </summary>
    public class TransitEncryptionKeyInfo
    {
        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>
        /// The type of the key.
        /// </value>
        [JsonProperty("type")]
        public TransitKeyType KeyType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether deletion of this key is allowed.
        /// </summary>
        /// <value>
        /// <c>true</c> if deletion allowed; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("deletion_allowed")]
        public bool IsDeletionAllowed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if key derivation MUST be used. 
        /// If enabled, all encrypt/decrypt requests to this named key must provide a context which is used for key derivation. 
        /// </summary>
        /// <value>
        /// <c>true</c> if derivation must be used; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("derived")]
        public bool MustUseKeyDerivation { get; set; }

        /// <summary>
        /// Gets or sets the key version creation times.
        /// Shows the creation time of each key version; the values are not the keys themselves. 
        /// </summary>
        /// <value>
        /// The key version creation times.
        /// </value>
        [JsonProperty("keys")]
        public Dictionary<string, int> KeyVersionCreationTimes { get; set; }

        /// <summary>
        /// Gets or sets the minimum version of ciphertext allowed to be decrypted. 
        /// Adjusting this as part of a key rotation policy can prevent old copies of ciphertext from being decrypted, 
        /// should they fall into the wrong hands. 
        /// </summary>
        /// <value>
        /// The minimum decryption version.
        /// </value>
        [JsonProperty("min_decryption_version")]
        public int MinimumDecryptionVersion { get; set; }

        /// <summary>
        /// Gets or sets the name of the encryption key.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [convergent encryption].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [convergent encryption]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("convergent_encryption")]
        public bool ConvergentEncryption { get; set; }

        /// <summary>
        /// Gets or sets the convergent encryption version.
        /// </summary>
        /// <value>
        /// The convergent encryption version.
        /// </value>
        [JsonProperty("convergent_encryption_version")]
        public int ConvergentEncryptionVersion { get; set; }

        /// <summary>
        /// Gets or sets the key derivation function.
        /// </summary>
        /// <value>
        /// The key derivation function.
        /// </value>
        [JsonProperty("kdf")]
        public string KeyDerivationFunction { get; set; }

        /// <summary>
        /// Gets or sets the latest version.
        /// </summary>
        /// <value>
        /// The latest version.
        /// </value>
        [JsonProperty("latest_version")]
        public int LatestVersion { get; set; }
    }
}