using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the generated Certificate.
    /// </summary>
    public abstract class AbstractCertificateData
    {
        /// <summary>
        /// Gets or sets the certificate format.
        /// </summary>
        /// <value>
        /// The certificate format.
        /// </value>
        [JsonIgnore]
        public CertificateFormat CertificateFormat { get; set; }

        /// <summary>
        /// Gets or sets the content of the certificate.
        /// </summary>
        /// <value>
        /// The content of the certificate.
        /// </value>
        [JsonProperty("certificate")]
        public string CertificateContent { get; set; }

        /// <summary>
        /// Gets or sets the content of the issuing ca certificate.
        /// </summary>
        /// <value>
        /// The content of the issuing ca certificate.
        /// </value>
        [JsonProperty("issuing_ca")]
        public string IssuingCACertificateContent { get; set; }

        /// <summary>
        /// Gets or sets the ca chain content.
        /// </summary>
        /// <value>
        /// The content of the ca chain.
        /// </value>
        [JsonProperty("ca_chain")]
        public string CAChainContent { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        /// <value>
        /// The serial number.
        /// </value>
        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }
    }
}