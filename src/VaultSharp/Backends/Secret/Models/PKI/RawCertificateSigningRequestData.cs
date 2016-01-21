using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the raw CSR.
    /// </summary>
    public class RawCertificateSigningRequestData
    {
        /// <summary>
        /// Gets or sets the encoded certificate format.
        /// </summary>
        /// <value>
        /// The encoded certificate format.
        /// </value>
        public CertificateFormat EncodedCertificateFormat { get; set; }

        /// <summary>
        /// Gets or sets the raw certificate signing request.
        /// </summary>
        /// <value>
        /// The raw certificate signing request.
        /// </value>
        [JsonProperty("csr")]
        public string RawCertificateSigningRequest { get; set; }

        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        [JsonProperty("private_key")]
        public string PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the type of the private key.
        /// </summary>
        /// <value>
        /// The type of the private key.
        /// </value>
        [JsonProperty("private_key_type")]
        public CertificateKeyType PrivateKeyType { get; set; }
    }
}