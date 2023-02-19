using System;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents information about the encryption key used by Vault.
    /// </summary>
    public class EncryptionKeyStatus
    {
        /// <summary>
        /// Gets or sets the install time for the key.
        /// </summary>
        /// <value>
        /// The install time.
        /// </value>
        [JsonPropertyName("install_time")]
        public DateTimeOffset InstallTime { get; set; }

        /// <summary>
        /// Gets or sets the sequential key number.
        /// </summary>
        /// <value>
        /// The sequential key number.
        /// </value>
        [JsonPropertyName("term")]
        public int SequentialKeyNumber { get; set; }
    }
}