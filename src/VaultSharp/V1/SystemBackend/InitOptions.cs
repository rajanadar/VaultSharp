﻿using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the initialization options for Vault.
    /// </summary>
    public class InitOptions
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the PGP keys.
        /// An array of PGP public keys used to encrypt the output unseal keys. 
        /// Ordering is preserved. The keys must be base64-encoded from their original binary representation. 
        /// The size of this array must be the same as <see cref="SecretShares"/>.
        /// </summary>
        /// <value>
        /// The PGP keys.
        /// </value>
        [JsonPropertyName("pgp_keys")]
        public string[] PgpKeys { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a PGP public key used to encrypt the initial root token. 
        /// The key must be base64-encoded from its original binary representation.
        /// </summary>
        /// <value>
        /// The root token pgp key.
        /// </value>
        [JsonPropertyName("root_token_pgp_key")]
        public string RootTokenPgpKey { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the number of shares to split the master key into.
        /// </summary>
        /// <value>
        /// The secret shares.
        /// </value>
        [JsonPropertyName("secret_shares")]
        public int SecretShares { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the secret threshold.
        /// The number of shares required to reconstruct the master key. 
        /// This must be less than or equal to <see cref="SecretShares"/>. 
        /// If using Vault HSM with auto-unsealing, this value must be the same as <see cref="SecretShares"/>.
        /// </summary>
        /// <value>
        /// The secret threshold.
        /// </value>
        [JsonPropertyName("secret_threshold")]
        public int SecretThreshold { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the stored shares.
        /// The number of shares that should be encrypted by the HSM and stored for auto-unsealing (Vault HSM only). 
        /// Currently must be the same as <see cref="SecretShares"/>.
        /// </summary>
        /// <remarks>
        /// Only supported on Vault Pro/Enterprise
        /// </remarks>
        /// <value>
        /// The stored shares.
        /// </value>
        [JsonPropertyName("stored_shares")]
        public int StoredShares { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the recovery shares.
        /// The number of shares to split the recovery key into (Vault HSM only).
        /// </summary>
        /// <remarks>
        /// Only supported on Vault Pro/Enterprise
        /// </remarks>
        /// <value>
        /// The recovery shares.
        /// </value>
        [JsonPropertyName("recovery_shares")]
        public int RecoveryShares { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the recovery threshold.
        /// The number of shares required to reconstruct the recovery key (Vault HSM only). 
        /// This must be less than or equal to <see cref="RecoveryShares"/>.
        /// </summary>
        /// <remarks>
        /// Only supported on Vault Pro/Enterprise
        /// </remarks>
        /// <value>
        /// The recovery threshold.
        /// </value>
        [JsonPropertyName("recovery_threshold")]
        public int RecoveryThreshold { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the recovery PGP keys.
        /// An array of PGP public keys used to encrypt the output recovery keys (Vault HSM only). 
        /// Ordering is preserved. The keys must be base64-encoded from their original binary representation. 
        /// The size of this array must be the same as <see cref="RecoveryShares"/>.
        /// </summary>
        /// <remarks>
        /// Only supported on Vault Pro/Enterprise
        /// </remarks>
        /// <value>
        /// The recovery PGP keys.
        /// </value>
        [JsonPropertyName("recovery_pgp_keys")]
        public string[] RecoveryPgpKeys { get; set; }
    }
}