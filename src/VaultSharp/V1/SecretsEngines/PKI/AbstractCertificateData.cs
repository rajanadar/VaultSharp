using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents a Certificate.
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
        [JsonPropertyName("certificate")]
        public string CertificateContent { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        /// <value>
        /// The serial number.
        /// </value>
        [JsonPropertyName("serial_number")]
        public string SerialNumber { get; set; }
    }
}