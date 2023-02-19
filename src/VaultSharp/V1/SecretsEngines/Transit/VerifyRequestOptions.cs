using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the options for a request to Vault to validate either an HMAC or a signature.
    /// </summary>
    /// <seealso cref="VaultSharp.V1.SecretsEngines.Transit.VerifySingleInput" />
    public class VerifyRequestOptions : VerifySingleInput
    {
        /// <summary>
        /// Gets or sets the list of input data to verify signatures or HMACs for.
        /// </summary>
        /// <value>The list of input data.</value>
        [JsonPropertyName("batch_input", NullValueHandling = NullValueHandling.Ignore)]
        public List<VerifySingleInput> BatchInput { get; set; }
    }

    /// <summary>
    /// Represents a single data value (HMAC or signature) to be verified.
    /// </summary>
    public class VerifySingleInput
    {
        [JsonIgnore]
        public TransitHashAlgorithm HashAlgorithm { get; set; } = TransitHashAlgorithm.SHA2_256;

        /// <summary>
        /// Gets or sets the base64 encoded input data to have its signature verified.
        /// </summary>
        /// <value>The base64 encoded input.</value>
        [JsonPropertyName("input")]
        public string Base64EncodedInput { get; set; }

        /// <summary>
        /// Gets or sets the signature to verify.
        /// </summary>
        /// <value>The signature.</value>
        [JsonPropertyName("signature", NullValueHandling = NullValueHandling.Ignore)]
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the HMAC to verify.
        /// </summary>
        /// <value>The HMAC.</value>
        [JsonPropertyName("hmac")]
        public string Hmac { get; set; }

        /// <summary>
        /// Gets or sets the base64 encoded key derivation context, for use with ECDSA keys only.
        /// </summary>
        /// <value>The base64 encoded key derivation context.</value>
        [JsonPropertyName("context", NullValueHandling = NullValueHandling.Ignore)]
        public string Base64EncodedKeyDerivationContext { get; set; }

        /// <summary>
        /// Gets or sets a value that should be set to <c>true</c> if the input is already hashed.
        /// </summary>
        /// <value><c>true</c> if prehashed; otherwise, <c>false</c>.</value>
        [JsonPropertyName("prehashed")]
        public bool PreHashed { get; set; }

        /// <summary>
        /// When using an RSA key, specifies which RSA signature algorithm to use for signature verification.
        /// </summary>
        /// <value>The signature algorithm to use for signing.</value>
        [JsonPropertyName("signature_algorithm", NullValueHandling = NullValueHandling.Ignore)]
        public SignatureAlgorithm? SignatureAlgorithm { get; set; } = Transit.SignatureAlgorithm.pss;

        /// <summary>
        /// When using an ECDSA key, specifies the way in which the signature should be unmarshaled.
        /// </summary>
        /// <value>The marshaling algorithm.</value>
        [JsonPropertyName("marshaling_algorithm", NullValueHandling = NullValueHandling.Ignore)]
        public MarshalingAlgorithm? MarshalingAlgorithm { get; set; } = Transit.MarshalingAlgorithm.asn1;

        [JsonPropertyName("salt_length")]
        public string SaltLength { get; set; } = SaltLengthType.auto.ToString();
    }
}