using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents the rekey progress.
    /// </summary>
    public class RekeyProgress
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RekeyProgress"/> is complete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if complete; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("complete")]
        public bool Complete { get; set; }

        /// <summary>
        /// Gets or sets the new master keys. (possibly pgp encrypted)
        /// </summary>
        /// <value>
        /// The master keys.
        /// </value>
        [JsonProperty("keys")]
        public string[] MasterKeys { get; set; }

        /// <summary>
        /// Gets or sets the new base 64 master keys. (possibly pgp encrypted)
        /// </summary>
        /// <value>
        /// The master keys.
        /// </value>
        [JsonProperty("keys_base64")]
        public string[] Base64MasterKeys { get; set; }

        /// <summary>
        /// Gets or sets the nonce for the current rekey operation..
        /// </summary>
        /// <value>
        /// The nonce.
        /// </value>
        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        /// Gets or sets the PGP finger prints if PGP keys are being used to encrypt the final shares.
        /// </summary>
        /// <value>
        /// The PGP finger prints.
        /// </value>
        [JsonProperty("pgp_fingerprints")]
        public string[] PGPFingerPrints { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the final keys will be backed up to physical storage.
        /// </summary>
        /// <value>
        ///   <c>true</c> if backup; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("backup")]
        public bool Backup { get; set; }
    }
}