using System;
using Newtonsoft.Json;

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
        [JsonProperty("revocation_time")]
        public int RevocationTime { get; set; }

        [JsonProperty("revocation_time_rfc3339")]
        public DateTimeOffset? RevocationTimeRFC3339 { get; set; }

        [JsonProperty("issuer_id")]
        public string RevocationIssuerId { get; set; }
    }
}