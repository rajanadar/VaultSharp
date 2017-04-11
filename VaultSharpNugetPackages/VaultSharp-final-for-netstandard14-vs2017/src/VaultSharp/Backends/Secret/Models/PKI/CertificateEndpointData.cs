using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PKI
{
    /// <summary>
    /// Represents the URLs encoded in the generated certificates.
    /// </summary>
    public class CertificateEndpointData
    {
        /// <summary>
        /// Gets or sets the issuing certificate endpoints.
        /// </summary>
        /// <value>
        /// The issuing certificate endpoints.
        /// </value>
        [JsonProperty("issuing_certificates")]
        public List<string> IssuingCertificateEndpoints { get; set; }

        /// <summary>
        /// Gets or sets the CRL distribution point endpoints.
        /// </summary>
        /// <value>
        /// The CRL distribution point endpoints.
        /// </value>
        [JsonProperty("crl_distribution_points")]
        public List<string> CRLDistributionPointEndpoints { get; set; }

        /// <summary>
        /// Gets or sets the ocsp server endpoints.
        /// </summary>
        /// <value>
        /// The ocsp server endpoints.
        /// </value>
        [JsonProperty("ocsp_servers")]
        public List<string> OCSPServerEndpoints { get; set; }
    }
}