
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the options for a request to Vault to perform a 
    /// cryptographic signature on an input data string.
    /// </summary>
    /// <seealso cref="SignSingleInput" />
    public class SignRequestOptions : SignSingleInput
    {
        /// <summary>
        /// Gets or sets the list of input data to calculate signatures for.
        /// </summary>
        /// <value>The list of input data.</value>
        [JsonProperty("batch_input")]
        public List<SignSingleInput> BatchInput { get; set; }
    }

    /// <summary>
    /// Represents a single data value to be signed.
    /// </summary>
    public class SignSingleInput
    {
        [JsonProperty("hash_algorithm")]
        public TransitHashAlgorithm HashAlgorithm { get; set; } = TransitHashAlgorithm.SHA2_256;

        /// <summary>
        /// Gets or sets the version of the key to use for the operation. Should only be set if an explicit version is required.
        /// </summary>
        /// <value>The key version to use for the operation.</value>
        [JsonProperty("key_version", NullValueHandling = NullValueHandling.Ignore)]
        public int? KeyVersion { get; set; }

        /// <summary>
        /// Gets or sets the base64 encoded input data to be signed.
        /// </summary>
        /// <value>The base64 encoded input.</value>
        [JsonProperty("input", NullValueHandling = NullValueHandling.Ignore)]
        public string Base64EncodedInput { get; set; }

        /// <summary>
        /// Gets or sets the base64 encoded key derivation context, for use with ECDSA keys only.
        /// </summary>
        /// <value>The base64 encoded key derivation context.</value>
        [JsonProperty("context", NullValueHandling = NullValueHandling.Ignore)]
        public string Base64EncodedKeyDerivationContext { get; set; }

        /// <summary>
        /// Gets or sets a value that should be set to <c>true</c> if the input is already hashed.
        /// </summary>
        /// <value><c>true</c> if prehashed; otherwise, <c>false</c>.</value>
        [JsonProperty("prehashed")]
        public bool PreHashed { get; set; }

        /// <summary>
        /// When using an RSA key, specifies which RSA signature algorithm to use for signing.
        /// </summary>
        /// <value>The signature algorithm to use for signing.</value>
        [JsonProperty("signature_algorithm", NullValueHandling = NullValueHandling.Ignore)]
        public SignatureAlgorithm? SignatureAlgorithm { get; set; } = Transit.SignatureAlgorithm.pss;

        /// <summary>
        /// When using an ECDSA key, specifies the way in which the signature should be marshaled.
        /// </summary>
        /// <value>The marshaling algorithm.</value>
        [JsonProperty("marshaling_algorithm", NullValueHandling = NullValueHandling.Ignore)]
        public MarshalingAlgorithm? MarshalingAlgorithm { get; set; } = Transit.MarshalingAlgorithm.asn1;

        [JsonProperty("salt_length")]
        public string SaltLength { get; set; } = SaltLengthType.auto.ToString();
    }

    /// <summary>
    /// The RSA signature algorithm to use for signing, if applicable.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SignatureAlgorithm
    {
        pss,
        pkcs1v15
    }

    /// <summary>
    /// The way in which the signature should be marshaled, if applicable.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MarshalingAlgorithm
    {
        asn1,
        jws
    }

    public enum SaltLengthType
    {
        auto,
        hash
    }
}