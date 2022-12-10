using System;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the raw certificate contents.
    /// </summary>
    public class RawCertificateData
    {
        /// <summary>
        /// Gets or sets the content of the certificate.
        /// </summary>
        /// <value>
        /// The content of the certificate.
        /// </value>
        [JsonProperty("certificate")]
        public string CertificateContent { get; set; }

        /// <summary>
        /// Gets or sets the revocation time.
        /// </summary>
        /// <value>
        /// The revocation time.
        /// </value>
        [JsonProperty("revocation_time")]
        public int RevocationTime { get; set; }

        [JsonProperty("revocation_time_rfc3339")]
        public DateTimeOffset? RevocationTimeRFC3339 { get; set; }

        [JsonProperty("issuer_id")]
        public string RevocationIssuerId { get; set; }

        /// <summary>
        /// Gets or sets the encoded certificate format.
        /// </summary>
        /// <value>
        /// The encoded certificate format.
        /// </value>
        public CertificateFormat EncodedCertificateFormat { get; set; }
    }
}