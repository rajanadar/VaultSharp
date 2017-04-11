using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the URLs encoded in the generated certificates.
    /// </summary>
    public class CertificateEndpointOptions
    {
        /// <summary>
        /// Gets or sets the issuing certificate endpoints.
        /// </summary>
        /// <value>
        /// The issuing certificate endpoints.
        /// </value>
        [JsonProperty("issuing_certificates")]
        public string IssuingCertificateEndpoints { get; set; }

        /// <summary>
        /// Gets or sets the CRL distribution point endpoints.
        /// </summary>
        /// <value>
        /// The CRL distribution point endpoints.
        /// </value>
        [JsonProperty("crl_distribution_points")]
        public string CRLDistributionPointEndpoints { get; set; }

        /// <summary>
        /// Gets or sets the ocsp server endpoints.
        /// </summary>
        /// <value>
        /// The ocsp server endpoints.
        /// </value>
        [JsonProperty("ocsp_servers")]
        public string OCSPServerEndpoints { get; set; }
    }
}