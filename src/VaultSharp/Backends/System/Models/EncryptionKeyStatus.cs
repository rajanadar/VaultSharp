using System;
using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents information about the encryption key used by Vault.
    /// </summary>
    public class EncryptionKeyStatus
    {
        /// <summary>
        /// Gets or sets the sequential key number.
        /// </summary>
        /// <value>
        /// The sequential key number.
        /// </value>
        [JsonProperty("term")]
        public int SequentialKeyNumber { get; set; }

        /// <summary>
        /// Gets or sets the install time for the key.
        /// </summary>
        /// <value>
        /// The install time.
        /// </value>
        [JsonProperty("install_time")]
        public DateTimeOffset InstallTime { get; set; }
    }
}