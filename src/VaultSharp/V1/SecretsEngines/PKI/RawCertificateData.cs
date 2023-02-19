using System;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("certificate")]
        public string CertificateContent { get; set; }

        /// <summary>
        /// Gets or sets the revocation time.
        /// </summary>
        /// <value>
        /// The revocation time.
        /// </value>
        [JsonPropertyName("revocation_time")]
        public int RevocationTime { get; set; }

        [JsonPropertyName("revocation_time_rfc3339")]
        public DateTimeOffset? RevocationTimeRFC3339 { get; set; }

        [JsonPropertyName("issuer_id")]
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