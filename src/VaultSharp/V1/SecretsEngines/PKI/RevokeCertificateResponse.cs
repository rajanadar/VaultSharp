using System;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI
{
    /// <summary>
    /// Represents the Certificate revocation response.
    /// </summary>
    public class RevokeCertificateResponse
    {
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
    }
}