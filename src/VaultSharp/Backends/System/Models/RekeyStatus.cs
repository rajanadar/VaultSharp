using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents the configuration and progress of the current rekey attempt.
    /// </summary>
    public class RekeyStatus
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RekeyStatus"/> is started.
        /// </summary>
        /// <value>
        ///   <c>true</c> if started; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("started")]
        public bool Started { get; set; }

        /// <summary>
        /// Gets or sets the nonce for the current rekey operation..
        /// </summary>
        /// <value>
        /// The nonce.
        /// </value>
        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        /// Gets or sets the number of shares required to reconstruct the master key.
        /// </summary>
        /// <value>
        /// The secret threshold.
        /// </value>
        [JsonProperty("t")]
        public int SecretThreshold { get; set; }

        /// <summary>
        /// Gets or sets the number of shares to split the master key into.
        /// </summary>
        /// <value>
        /// The secret shares.
        /// </value>
        [JsonProperty("n")]
        public int SecretShares { get; set; }

        /// <summary>
        /// Gets or sets the number of unseal keys provided for this rekey.
        /// </summary>
        /// <value>
        /// The progress.
        /// </value>
        [JsonProperty("progress")]
        public int UnsealKeysProvided { get; set; }

        /// <summary>
        /// Gets or sets the required number of unseal keys required to complete the rekeying process.
        /// </summary>
        /// <value>
        /// The required unseal keys.
        /// </value>
        [JsonProperty("required")]
        public int RequiredUnsealKeys { get; set; }

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