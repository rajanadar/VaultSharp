using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents the Rekey backup information.
    /// </summary>
    public class RekeyBackupInfo
    {
        /// <summary>
        /// Gets or sets the nonce for the current rekey operation..
        /// </summary>
        /// <value>
        /// The nonce.
        /// </value>
        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        /// Gets or sets the map of PGP key fingerprint to hex-encoded PGP-encrypted key.
        /// </summary>
        /// <value>
        /// The map of PGP key fingerprint to hex-encoded PGP-encrypted key.
        /// </value>
        [JsonProperty("keys")]
        public Dictionary<string, string> PGPFingerprintToEncryptedKeyMap { get; set; }
    }
}