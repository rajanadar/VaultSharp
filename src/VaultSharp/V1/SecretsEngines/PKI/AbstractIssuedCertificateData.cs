using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the generated Certificate.
    /// </summary>
    public abstract class AbstractIssuedCertificateData : AbstractCertificateData
    {
        /// <summary>
        /// Gets or sets the content of the issuing ca certificate.
        /// </summary>
        /// <value>
        /// The content of the issuing ca certificate.
        /// </value>
        [JsonPropertyName("issuing_ca")]
        public string IssuingCACertificateContent { get; set; }

        /// <summary>
        /// Gets or sets the ca chain content.
        /// </summary>
        /// <value>
        /// The content of the ca chain.
        /// </value>
        [JsonPropertyName("ca_chain")]
        public string[] CAChainContent { get; set; }
    }
}