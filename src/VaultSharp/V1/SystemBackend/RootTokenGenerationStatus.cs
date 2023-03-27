﻿using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the configuration and progress of the root token generation attempt.
    /// </summary>
    public class RootTokenGenerationStatus
    {
        /// <summary>
        /// Gets or sets the nonce for the current root token generation.
        /// </summary>
        /// <value>
        /// The nonce.
        /// </value>
        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        /// Gets or sets a value indicating this <see cref="RootTokenGenerationStatus"/>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if started; otherwise, <c>false</c>.
        /// </value>
        [JsonPropertyName("started")]
        public bool Started { get; set; }

        /// <summary>
        /// Gets or sets the number of unseal keys provided for this root token generation.
        /// </summary>
        /// <value>
        /// The progress.
        /// </value>
        [JsonPropertyName("progress")]
        public int UnsealKeysProvided { get; set; }

        /// <summary>
        /// Gets or sets the required number of unseal keys required to 
        /// complete the root token generation.
        /// </summary>
        /// <value>
        /// The required unseal keys.
        /// </value>
        [JsonPropertyName("required")]
        public int RequiredUnsealKeys { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// this root token generation is complete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if complete; otherwise, <c>false</c>.
        /// </value>
        [JsonPropertyName("complete")]
        public bool Complete { get; set; }

        /// <summary>
        /// Gets or sets the encoded token if the attempt is complete.
        /// </summary>
        /// <value>
        /// The encoded token.
        /// </value>
        [JsonPropertyName("encoded_token")]
        public string EncodedToken { get; set; }

        /// <summary>
        /// Gets or sets the encoded root token if the attempt is complete.
        /// </summary>
        /// <value>
        /// The encoded root token.
        /// </value>
        [JsonPropertyName("encoded_root_token")]
        public string EncodedRootToken { get; set; }

        /// <summary>
        /// Gets or sets the PGP finger print if 
        /// a PGP key is being used to encrypt the final root token.
        /// </summary>
        /// <value>
        /// The PGP finger prints.
        /// </value>
        [JsonPropertyName("pgp_fingerprint")]
        public string PGPFingerPrint { get; set; }

        /// <summary>
        /// Gets or sets the OTP.
        /// </summary>
        /// <value>
        /// The OTP.
        /// </value>
        [JsonPropertyName("otp")]
        public string OTP { get; set; }

        /// <summary>
        /// Gets or sets the OTP length.
        /// </summary>
        /// <value>
        /// The OTP length.
        /// </value>
        [JsonPropertyName("otp_length")]
        public int OTPLength { get; set; }
    }
}